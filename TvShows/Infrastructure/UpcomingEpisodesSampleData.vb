Imports System.ComponentModel
Imports TvShows.Model

Namespace Infrastructure

    Public Class UpcomingEpisodesSampleData

        Public Property EpisodesCollectionView As ICollectionView

        Public Sub New()
            Dim tvShow1 As New TvShow() With {.Name = "Black Sails", .Id = 12345}
            Dim tvShow2 As New TvShow() With {.Name = "Homeland", .Id = 54321}

            Dim episodes = New List(Of Episode) From {
                New Episode With {.Name = "", .TvShow = tvShow1},
                New Episode With {.Name = "", .TvShow = tvShow1}
            }

            EpisodesCollectionView = New ListCollectionView(episodes)
            EpisodesCollectionView.GroupDescriptions.Add(New PropertyGroupDescription("Name"))

        End Sub

    End Class

End Namespace
