Imports TvShows.DAL
Imports TvShows.Model

Namespace BLL

    Public Class BuinessLayer
        Implements IBusinessLayer
        Private _tvshowRepo As ITvShowRepository
        Private _episodeRepo As IEpisodeRepository

        Public Sub New()
            _tvshowRepo = New TvShowRepository()
            _episodeRepo = New EpisodeRepository()
        End Sub

        Public Sub New(tvshowRepo As ITvShowRepository, episodeRepo As IEpisodeRepository)
            _tvshowRepo = tvshowRepo
            _episodeRepo = episodeRepo
        End Sub

#Region " TV Show Methods "

        Public Function GetAllTvShows() As IList(Of TvShow) Implements IBusinessLayer.GetAllTvShows
            Return _tvshowRepo.GetAll(Function(e) e.Episodes)
        End Function

        Public Function GetTvShowById(tvshowId As Integer) As TvShow Implements IBusinessLayer.GetTvShowById
            Return _tvshowRepo.GetSingle(Function(t) t.Id.Equals(tvshowId), Function(t) t.Episodes)
        End Function

        Public Function AddTvShow(tvshow As TvShow) As Infrastructure.ActionResult Implements IBusinessLayer.AddTvShow
            tvshow.EntityState = EntityState.Added
            If tvshow.Episodes IsNot Nothing Then
                For Each ep In tvshow.Episodes
                    ep.EntityState = EntityState.Added
                Next
            End If
            Return _tvshowRepo.Add(tvshow)
        End Function

        Public Function RemoveTvShow(tvshow As TvShow) As Infrastructure.ActionResult Implements IBusinessLayer.RemoveTvShow
            tvshow.EntityState = EntityState.Deleted
            For Each ep In tvshow.Episodes
                ep.EntityState = EntityState.Deleted
            Next
            Return _tvshowRepo.Remove(tvshow)
        End Function

        Public Function UpdateTvShow(tvshow As TvShow) As Infrastructure.ActionResult Implements IBusinessLayer.UpdateTvShow
            'tvshow.EntityState = EntityState.Modified
            'For Each ep In tvshow.Episodes
            '    ep.EntityState = EntityState.Modified
            'Next
            Return _tvshowRepo.Update(tvshow)
        End Function

#End Region

#Region " Episode Methods "

        Public Function GetEpisodesByTvShowId(tvshowId As Integer) As IList(Of Episode) Implements IBusinessLayer.GetEpisodesByTvShowId
            Return _episodeRepo.GetList(Function(e) e.TvShow.Id.Equals(tvshowId))
        End Function

        Public Function AddEpisode(episode As Episode) As Infrastructure.ActionResult Implements IBusinessLayer.AddEpisode
            episode.EntityState = EntityState.Added
            Return _episodeRepo.Add(episode)
        End Function

        Public Function RemoveEpisode(episode As Episode) As Infrastructure.ActionResult Implements IBusinessLayer.RemoveEpisode
            episode.EntityState = EntityState.Deleted
            Return _episodeRepo.Remove(episode)
        End Function

        Public Function UpdateEpisode(episode As Episode) As Infrastructure.ActionResult Implements IBusinessLayer.UpdateEpisode
            episode.EntityState = EntityState.Modified
            Return _episodeRepo.Update(episode)
        End Function

        Public Function GetEpisodesAvailableNotCollected() As IList(Of Episode) Implements IBusinessLayer.GetEpisodesAvailableNotCollected
            Return _episodeRepo.GetList(Function(e) e.IsAvailable = True AndAlso e.IsCollected = False)
        End Function

        Public Function GetEpisodeList(where As Func(Of Episode, Boolean)) As IList(Of Episode) Implements IBusinessLayer.GetEpisodeList
            Return _episodeRepo.GetList(where, Function(e) e.TvShow)
        End Function

        Public Function GetAllEpisodes() As IList(Of Episode) Implements IBusinessLayer.GetAllEpisodes
            Return _episodeRepo.GetAll(Function(e) e.TvShow)
        End Function

#End Region

    End Class

End Namespace
