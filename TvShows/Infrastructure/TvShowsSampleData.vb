Imports TvShows.Model

Namespace Infrastructure

    Public Class TvShowsSampleData

        Public Property TvShows As IList(Of TvShow)

        Public Sub New()
            TvShows = New List(Of TvShow)

            Dim show1 As New TvShow() With {
                .Id = 12345,
                .Name = "Jekyll and Hyde",
                .PosterPath = "/jemsa3TTYZx49oyzpnLVBWsVW5p.jpg",
                .IsFavorite = True,
                .Episodes = New List(Of Episode) From {
                    New Episode With {
                        .Name = "Black Dog",
                        .AirDateString = "2015-11-29",
                        .EpisodeNumber = 5,
                        .FilePath = "/Jekyll and Hyde/Season 01/Jekyll.and.Hyde.S01E05.mkv",
                        .IsArchived = True,
                        .Overview = "Robert discovers he has some distant relations, so he and Max travel to the remote village where they live in the hope they will be able to tell him more about the Jekyll curse. On their arrival, they discover several mysterious deaths have occurred recently and the locals believe a legendary demonic dog is responsible. Robert discovers an ancient book that seems to reveal a connection between his relatives and Tenebrae, and suspects they are dealing with a shape-shifter.",
                        .SeasonNumber = 1,
                        .StillPath = "/yeE6v1enlYYdR6sKHJ6ckFth7OO.jpg"},
                    New Episode With {
                        .Name = "",
                        .AirDateString = "",
                        .EpisodeNumber = 6,
                        .FilePath = "",
                        .IsArchived = True,
                        .Overview = "",
                        .SeasonNumber = 2,
                        .StillPath = ""}
                }
            }
            TvShows.Add(show1)

            Dim show2 As New TvShow() With {.Id = 22445, .Name = "Show 2", .PosterPath = ""}
            TvShows.Add(show2)

            TvShowsCollectionView = New CollectionView(TvShows)
        End Sub

        Public Property TvShowsCollectionView As ComponentModel.ICollectionView

    End Class

End Namespace
