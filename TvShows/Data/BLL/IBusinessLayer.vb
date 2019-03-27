Namespace BLL

    Public Interface IBusinessLayer

        Function GetAllTvShows() As IList(Of Model.TvShow)
        Function GetTvShowById(tvshowId As Integer) As Model.TvShow
        Function AddTvShow(tvshow As Model.TvShow) As Infrastructure.ActionResult
        Function UpdateTvShow(tvshow As Model.TvShow) As Infrastructure.ActionResult
        Function RemoveTvShow(tvshow As Model.TvShow) As Infrastructure.ActionResult
        'Sub AddTvShow(ParamArray tvshow As Model.TvShow())
        'Sub UpdateTvShow(ParamArray tvshow As Model.TvShow())
        'Sub RemoveTvShow(ParamArray tvshow As Model.TvShow())

        Function GetEpisodesByTvShowId(tvshowId As Integer) As IList(Of Model.Episode)
        Function GetEpisodeList(where As Func(Of Model.Episode, Boolean)) As IList(Of Model.Episode)
        Function GetEpisodesAvailableNotCollected() As IList(Of Model.Episode)
        Function GetAllEpisodes() As IList(Of Model.Episode)
        Function AddEpisode(episode As Model.Episode) As Infrastructure.ActionResult
        Function UpdateEpisode(episode As Model.Episode) As Infrastructure.ActionResult
        Function RemoveEpisode(episode As Model.Episode) As Infrastructure.ActionResult

    End Interface

End Namespace
