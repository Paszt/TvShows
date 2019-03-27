Namespace DAL

    Public Interface ITvShowRepository
        Inherits IGenericDataRepository(Of Model.TvShow)
    End Interface

    Public Class TvShowRepository
        Inherits GenericDataRepository(Of Model.TvShow)
        Implements ITvShowRepository
    End Class

    Public Interface IEpisodeRepository
        Inherits IGenericDataRepository(Of Model.Episode)
    End Interface

    Public Class EpisodeRepository
        Inherits GenericDataRepository(Of Model.Episode)
        Implements IEpisodeRepository
    End Class

End Namespace
