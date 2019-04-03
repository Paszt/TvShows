Imports System.Text.RegularExpressions

Namespace Model

    Partial Public Class TvShow

        Const videoExtensionRegExPattern =
            "\.avi|\.mkv|\.mpg|\.mpeg|\.wmv|\.ogm|\.mp4|\.iso|" &
            "\.img|\.divx|\.m2ts|\.m4v|\.ts|\.flv|\.f4v|\.mov|" &
            "\.rmvb|\.vob|\.dvr-ms|\.wtv|\.ogv|\.3gp|\.webm"

        'Private Shared ReadOnly VideoExtensions As String() = New String() {
        '    ".avi", ".mkv", ".mpg", ".mpeg", ".wmv",
        '    "ogm", ".mp4", ".iso", ".img", ".divx",
        '    "m2ts", ".m4v", ".ts", ".flv", ".f4v",
        '    "mov", ".rmvb", ".vob", ".dvr-ms",
        '    ".wtv", "ogv", ".3gp", ".webm"
        '}

        Public Function FullUpdate() As Infrastructure.ActionResult
            Dim bl As New BLL.BuinessLayer()
            Dim result As New Infrastructure.ActionResult()
            Dim sb As New Text.StringBuilder("Updating """ & Name & """ episodes.")
            Dim updatedTvShow As TvShow
            updatedTvShow = Tmdb.Api.GetTvShowWithEpisodes(Id.Value)

            If Not OnlineInfoEquals(updatedTvShow) Then
                HomePage = updatedTvShow.HomePage
                Name = updatedTvShow.Name
                OriginCountry = updatedTvShow.OriginCountry
                Overview = updatedTvShow.Overview
                PosterPath = updatedTvShow.PosterPath
                BackdropPath = updatedTvShow.BackdropPath
                EntityState = EntityState.Modified
            End If

            For Each currentEp In Episodes
                ' removed episodes that have been deleted online
                If updatedTvShow.GetEpisodeBySeasonEpisode(currentEp.SeasonNumber, currentEp.EpisodeNumber) Is Nothing Then
                    If Not String.IsNullOrWhiteSpace(currentEp.FilePath) Then
                        sb.AppendLine("  " & currentEp.SeasonEpisodeString & " was deleted online but a local file exists (" & currentEp.FilePath & ")")
                    Else
                        currentEp.EntityState = EntityState.Deleted
                        sb.AppendLine("  " & currentEp.SeasonEpisodeString & " was removed from local since it was deleted online.")
                        'bl.RemoveEpisode(currentEp)
                    End If
                End If
            Next

            For Each ep In updatedTvShow.Episodes
                'update episode information
                Dim curEp = GetEpisodeBySeasonEpisode(ep.SeasonNumber, ep.EpisodeNumber)
                If curEp IsNot Nothing Then
                    'update base information if needed
                    curEp.Update(ep)
                Else
                    Dim epi As Episode = Episode.NewFromRemote(ep, Me)
                    Episodes.Add(epi)
                End If
            Next

            result.Message = sb.ToString()

            Dim updateResult = bl.UpdateTvShow(Me)
            If Not updateResult.Succeeded Then
                result.Message &= Environment.NewLine & updateResult.Message
            End If
            'update episodes' local file paths
            updateResult = UpdateEpisodeFilePaths(True, result)

            Return result
        End Function

        Public Function UpdateEpisodeFilePaths(Optional updateDatabase As Boolean = True, Optional result As Infrastructure.ActionResult = Nothing) As Infrastructure.ActionResult
            If result Is Nothing Then
                result = New Infrastructure.ActionResult()
            End If

            If Not IO.Directory.Exists(FullDirectoryPath) Then
                Return result
            End If

            For Each ep In Episodes
                If Not String.IsNullOrWhiteSpace(ep.FilePath) AndAlso Not IO.File.Exists(ep.FullFilePath) Then
                    result.Message &= "Previously existing file """ & ep.FullFilePath & """ no longer exists." & Environment.NewLine
                    ep.FilePath = Nothing
                End If
            Next

            Dim reSearchPattern = New Regex(videoExtensionRegExPattern, RegexOptions.IgnoreCase)
            Dim videoFilePaths = IO.Directory.EnumerateFiles(FullDirectoryPath, "*", IO.SearchOption.AllDirectories).
                Where(Function(file) reSearchPattern.IsMatch(IO.Path.GetExtension(file)))

            Dim vfnp As New Infrastructure.VideoFileNameParser(True)

            'TODO: finish Model.TvShow.UpdateEpisodeFilePaths
            For Each videoFilePath In videoFilePaths
                Dim vfnpResult = vfnp.Parse(videoFilePath)
                If vfnpResult.SeasonNumber.HasValue AndAlso vfnpResult.EpisodeNumbers IsNot Nothing AndAlso vfnpResult.EpisodeNumbers.Count > 0 Then
                    For Each epNum In vfnpResult.EpisodeNumbers
                        Dim ep = Episodes.Where(Function(e) e.SeasonNumber = vfnpResult.SeasonNumber.Value AndAlso e.EpisodeNumber = epNum).FirstOrDefault()
                        If ep Is Nothing Then
                            result.Message &= Environment.NewLine & "No episode found for file " & vfnpResult.OriginalName
                        Else
                            ep.FilePath = videoFilePath.Replace(My.Settings.TvShowRootPath, String.Empty)
                        End If
                    Next
                End If
            Next

            If updateDatabase Then
                Dim bl As New BLL.BuinessLayer
                Dim updateResult = bl.UpdateTvShow(Me)
                result.Message &= Environment.NewLine & updateResult.Message
            End If
            Return result
        End Function

        Public Function GetEpisodeBySeasonEpisode(seasonNo As Integer, episodeNo As Integer) As Episode
            If Episodes Is Nothing Then
                Return Nothing
            Else
                Return Episodes.Where(Function(e) e.SeasonNumber = seasonNo And e.EpisodeNumber = episodeNo).FirstOrDefault()
            End If
        End Function

        Public Function RepairEpisodes() As Infrastructure.ActionResult
            Dim result = New Infrastructure.ActionResult()
            'Dim dupes = Episodes.
            '               GroupBy(Function(e) New With {e.SeasonNumber, e.EpisodeNumber}).
            '               Where(Function(g) g.Count() >= 1).
            '               Select(Function(g) g.Key).ToList()
            'Stop
            'For Each epDupe In dupes

            'Next

            Dim dupIds As New List(Of Integer)
            For Each ep In Episodes
                Dim eps = Episodes.Where(Function(e) e.EpisodeNumber = ep.EpisodeNumber AndAlso e.SeasonNumber = ep.SeasonNumber).ToList()
                If eps.Count > 1 Then
                    For Each e In eps.Skip(1)
                        dupIds.Add(e.Id.Value)
                    Next
                End If
            Next

            For Each Id As Integer In dupIds.Distinct()
                Episodes.Where(Function(e) e.Id.Value = Id).First().EntityState = EntityState.Deleted
            Next

            Dim bl As New BLL.BuinessLayer
            Dim updateResult = bl.UpdateTvShow(Me)
            If Not updateResult.Succeeded Then
                result.Message = updateResult.Message
            End If

            Return result
        End Function

    End Class

End Namespace
