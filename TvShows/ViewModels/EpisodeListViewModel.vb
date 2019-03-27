Imports System.Collections.ObjectModel
Imports TvShows.Infrastructure

Namespace ViewModels

    Public Class EpisodeListViewModel
        Inherits NotifyPropertyChanged

        Public Sub New()
        End Sub

        Public Sub New(viewTitle As String, predicate As Func(Of Model.Episode, Boolean))
            Title = viewTitle
            Dim bl As New BLL.BuinessLayer
            Episodes = CType(bl.GetEpisodeList(predicate), List(Of Model.Episode))

            'Dim showlist = (From e In Episodes
            '                Select e.TvShow).OrderBy(Function(s) s.Name).ToList()

            TvShows = New List(Of Model.TvShow)
            For Each ep In Episodes
                Dim epShow = TvShows.FirstOrDefault(Function(s) CBool(s.Id = ep.TvShow.Id))
                If epShow Is Nothing Then
                    epShow = ep.TvShow
                    TvShows.Add(epShow)
                End If
                epShow.Episodes.Add(ep)
            Next

        End Sub

        Private _Title As String
        Public Property Title As String
            Get
                Return _Title
            End Get
            Set(value As String)
                SetProperty(_Title, value)
            End Set
        End Property

        Private _Episodes As New List(Of Model.Episode)
        Public Property Episodes As List(Of Model.Episode)
            Get
                Return _Episodes
            End Get
            Set(value As List(Of Model.Episode))
                SetProperty(_Episodes, value)
            End Set
        End Property

        'Private _EpisodesListView As ListCollectionView
        'Public Property EpisodesListView As ListCollectionView
        '    Get
        '        Return _EpisodesListView
        '    End Get
        '    Set(value As ListCollectionView)
        '        SetProperty(_EpisodesListView, value)
        '    End Set
        'End Property

        Private _tvShows As List(Of Model.TvShow)
        Public Property TvShows As List(Of Model.TvShow)
            Get
                Return _tvShows
            End Get
            Set(value As List(Of Model.TvShow))
                SetProperty(_tvShows, value)
            End Set
        End Property

        Public ReadOnly Property CloseCommand As ICommand
            Get
                Return New RelayCommand(Sub()
                                            My.Application.ShowMainView()
                                        End Sub)
            End Get
        End Property

    End Class

End Namespace
