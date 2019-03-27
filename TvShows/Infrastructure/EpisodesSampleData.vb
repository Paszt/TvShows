Imports TvShows.Model

Namespace Infrastructure

    Public Class EpisodesSampleData

        Public Property EpisodesCollectionView As ListCollectionView

        Public Sub New()
            Dim episodes = New List(Of Episode) From {
                New Episode With {.SeasonNumber = 8, .EpisodeNumber = 1, .AirDateString = "2014-08-23", .IsArchived = True,
                                  .Name = "Deep Breath", .StillPath = "/fn0tmqmL7P83DwLksG7TuFHNaoV.jpg", .FilePath = "Dr Who.S08E01.x264.DESTROYER.mkv",
                                  .Overview = "When a dinosaur materialises alongside the Houses of Parliament in Victorian London, the Doctor's old friends, the Paternoster Gang, are relieved when he arrives, seemingly to deal with the creature. However, they soon realise that the Doctor is the one in need of help; newly regenerated, extremely volatile and questioning his self-worth, this is a very different man to the one they last saw. The only person that may be able to help him is Clara, and she is still grappling with losing the Doctor she knew and loved."},
                New Episode With {.SeasonNumber = 8, .EpisodeNumber = 2, .AirDateString = "2014-08-30", .IsArchived = False, .IsSelected = True,
                                  .Name = "Into the Dalek", .StillPath = "/bR45NIPLU4wkaHK2KIIAKJUVHmp.jpg",
                                  .Overview = "A Dalek fleet surrounds a lone rebel ship, and only the Doctor can help them now. The Doctor faces his greatest enemy and he needs Clara by his side. Confronted with a decision that could change the Daleks forever he is forced to examine his conscience. Will he find the answer to his question - ""Am I a good man?"""},
                New Episode With {.SeasonNumber = 8, .EpisodeNumber = 3, .AirDateString = "2014-09-06", .IsArchived = False,
                                  .Name = "Robot of Sherwood", .StillPath = "/8bMdhKODbWluNpKnsDwsCKTWQNH.jpg",
                                  .Overview = "In a sun-dappled Sherwood Forest, The Doctor discovers an evil plan from beyond the stars. But with the fate of Nottingham at stake (and possibly Derby), there's no time for the two adventurers to get into a fight about who is real and who isn't - which is probably why they do very little else!"},
                New Episode With {.SeasonNumber = 9, .EpisodeNumber = 1, .AirDateString = "2015-09-19", .IsArchived = True,
                                  .Name = "The Magician's Apprentice (1)", .StillPath = "/4szgVi355DvGfmSkQulNYWIkbIP.jpg",
                                  .Overview = "Where is the Doctor? When the skies of Earth are frozen by a mysterious alien force, Clara needs her friend. But where is the Doctor, and what is he hiding from? As past deeds come back to haunt him, old enemies will come face-to-face, and for the Doctor and Clara survival seems impossible."},
                New Episode With {.SeasonNumber = 9, .EpisodeNumber = 2, .AirDateString = "2015-09-26", .IsArchived = True,
                                  .Name = "The Witch's Familiar (2)", .StillPath = "/tv0WKdJYAkFZM34obgEzeJyRvEi.jpg", .FilePath = "Dr Who.S09E02.720.HDTV.OLYMPIC.avi",
                                  .Overview = "Trapped and alone in a terrifying Dalek city, the Doctor is at the heart of an evil Empire; no sonic, no TARDIS, nobody to help. With his greatest temptation before him, can the Doctor resist? And will there be mercy?"}
            }

            EpisodesCollectionView = New ListCollectionView(episodes)
            EpisodesCollectionView.GroupDescriptions.Add(New PropertyGroupDescription("SeasonNumber"))
            EpisodesCollectionView.CustomSort = New EpisodeSorter()

        End Sub

    End Class

End Namespace

