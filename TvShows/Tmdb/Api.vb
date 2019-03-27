Option Strict On

Namespace Tmdb

    Public Class Api
        ''Implements IDisposable

        Public Const Key As String = "03fab28c05313508eae7d69064ba1612"

        'Private rg As Infrastructure.RateGate

        Public Shared Property Language As Iso639_1.Code = Iso639_1.Code.en

        'Public Sub New()
        '    Language = Iso639_1.Code.en
        '    'rg = New Infrastructure.RateGate(38, TimeSpan.FromSeconds(10))
        'End Sub

        'Public Sub New(LanguageCode As Iso639_1.Code)
        '    Me.New()
        '    Language = LanguageCode
        'End Sub

        Public Shared Function GetTvShow(id As Integer) As Model.TvShow
            Dim responseJsonString = GetApiResponse(Urls.TvShow(id, Language))
            Return responseJsonString.FromJSON(Of Model.TvShow)()
        End Function

        Public Shared Function GetTvSeason(TmdbShowId As Integer, SeasonNumber As Integer) As Model.Season
            Dim responseJsonString = GetApiResponse(Urls.TvSeason(TmdbShowId, SeasonNumber, Language))
            Return responseJsonString.FromJSON(Of Model.Season)
        End Function

        Public Shared Function GetTvShowWithEpisodes(id As Integer) As Model.TvShow
            Dim ts = GetTvShow(id)
            If ts.Episodes Is Nothing Then
                ts.Episodes = New List(Of Model.Episode)
            End If
            For Each tvShowSeason In ts.Seasons
                Dim ss = GetTvSeason(id, tvShowSeason.SeasonNumber)
                For Each ep In ss.Episodes
                    ts.Episodes.Add(ep)
                Next
            Next
            Return ts
        End Function

        Public Shared Function TvShowSearch(query As String) As List(Of Model.TvShow)
            Dim responseJsonString = GetApiResponse(Urls.TvShowSearch(query, Language))
            Dim tsrp = responseJsonString.FromJSON(Of Tmdb.TvSearchResultPage)
            Return tsrp.Results
        End Function

        Public Shared Function TvShowSearch(query As String, year As Integer) As List(Of Model.TvShow)
            Dim responseJsonString = GetApiResponse(Urls.TvShowSearch(query, Language, year))
            Dim tsrp = responseJsonString.FromJSON(Of Tmdb.TvSearchResultPage)
            Return tsrp.Results
        End Function

        Public Shared Function GetConfiguration(Optional forceUpdate As Boolean = False) As Configuration
            Dim responseJsonString = My.Settings.TmdbConfiguration
            If forceUpdate OrElse String.IsNullOrEmpty(responseJsonString) Then
                responseJsonString = GetApiResponse(Urls.Configuration)
                My.Settings.TmdbConfiguration = responseJsonString
            End If
            Return responseJsonString.FromJSON(Of Configuration)
        End Function

#Region " Helper Methods "

        Private Shared Function GetApiResponse(url As String) As String
            My.Application.rg.WaitToProceed()
            Return Infrastructure.WebResources.DownloadString(url)
        End Function

#End Region

        '#Region "IDisposable Support"
        '        Private disposedValue As Boolean ' To detect redundant calls

        '        ' IDisposable
        '        Protected Overridable Sub Dispose(disposing As Boolean)
        '            If Not disposedValue Then
        '                If disposing Then
        '                    If rg IsNot Nothing Then
        '                        rg.Dispose()
        '                    End If
        '                End If

        '                ' free unmanaged resources (unmanaged objects) and override Finalize() below.
        '                ' set large fields to null.
        '            End If
        '            disposedValue = True
        '        End Sub

        '        ' override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        '        'Protected Overrides Sub Finalize()
        '        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '        '    Dispose(False)
        '        '    MyBase.Finalize()
        '        'End Sub

        '        ' This code added by Visual Basic to correctly implement the disposable pattern.
        '        Public Sub Dispose() Implements IDisposable.Dispose
        '            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '            Dispose(True)
        '            ' uncomment the following line if Finalize() is overridden above.
        '            ' GC.SuppressFinalize(Me)
        '        End Sub
        '#End Region

    End Class

End Namespace
