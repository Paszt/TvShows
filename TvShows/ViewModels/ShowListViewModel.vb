Option Strict On
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports TvShows.Infrastructure
Imports TvShows.Model

Namespace ViewModels

    Public Class ShowListViewModel
        Inherits NotifyPropertyChanged

        Private innerTvShows As ObservableCollection(Of TvShow)
        Private TvShowsCollectionViewSource As CollectionViewSource

        Public Sub New()
            _showCompleteShows = My.Settings.ShowCompleteShows
            _showOnlyFavorites = My.Settings.ShowOnlyFavorites
        End Sub

        Public Sub RefreshShows()
            Dim th As New Threading.Thread(
                Sub()
                    IsLoading = True
                    Dim bl As New BLL.BuinessLayer
                    If Not DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
                        innerTvShows = New ObservableCollection(Of TvShow)(bl.GetAllTvShows().OrderBy(
                                                                           Function(t) If(t.Name.StartsWith("A ", StringComparison.OrdinalIgnoreCase) OrElse
                                                                               t.Name.StartsWith("The ", StringComparison.OrdinalIgnoreCase),
                                                                               t.Name.Substring(t.Name.IndexOf(" ") + 1),
                                                                               t.Name)))
                        InitializeTvShowsCollecionViewSource()
                    End If
                    IsLoading = False
                End Sub)
            th.Start()
        End Sub

        Private Sub InitializeTvShowsCollecionViewSource()
            Windows.Application.Current.Dispatcher.Invoke(
                Sub()
                    TvShowsCollectionViewSource = New CollectionViewSource() With {.Source = innerTvShows}
                    AddHandler TvShowsCollectionViewSource.Filter, AddressOf TvShowsCollection_Filter
                    OnPropertyChanged("TvShowsCollectionView")
                End Sub)
        End Sub

#Region " Properties "

        Public ReadOnly Property TvShowsCollectionView As ICollectionView
            Get
                If TvShowsCollectionViewSource IsNot Nothing Then
                    Return TvShowsCollectionViewSource.View
                End If
                Return Nothing
            End Get
        End Property

        Private _IsLoading As Boolean = False
        Public Property IsLoading As Boolean
            Get
                Return _IsLoading
            End Get
            Set(value As Boolean)
                If SetProperty(_IsLoading, value) Then
                    OnPropertyChanged("IsNotLoading")
                End If
            End Set
        End Property

        Private _IsNotLoading As Boolean
        Public ReadOnly Property IsNotLoading As Boolean
            Get
                Return Not IsLoading
            End Get
        End Property

        Private _showCompleteShows As Boolean
        Public Property ShowCompleteShows As Boolean
            Get
                Return _showCompleteShows
            End Get
            Set(value As Boolean)
                If SetProperty(_showCompleteShows, value) Then
                    My.Settings.ShowCompleteShows = value
                    OnFilterChanged()
                End If
            End Set
        End Property

        Private Sub OnFilterChanged()
            If TvShowsCollectionViewSource IsNot Nothing AndAlso
               TvShowsCollectionViewSource.View IsNot Nothing Then
                TvShowsCollectionViewSource.View.Refresh()
            End If
        End Sub

        Private _showOnlyFavorites As Boolean
        Public Property ShowOnlyFavorites As Boolean
            Get
                Return _showOnlyFavorites
            End Get
            Set(value As Boolean)
                If SetProperty(_showOnlyFavorites, value) Then
                    My.Settings.ShowOnlyFavorites = value
                    OnFilterChanged()
                End If
            End Set
        End Property

        Private _isUpdatingShows As Boolean
        Public Property IsUpdatingShows As Boolean
            Get
                Return _isUpdatingShows
            End Get
            Set(value As Boolean)
                SetProperty(_isUpdatingShows, value)
            End Set
        End Property

        Private _textFilter As String
        Public Property TextFilter As String
            Get
                Return _textFilter
            End Get
            Set(value As String)
                If SetProperty(_textFilter, value) Then
                    OnFilterChanged()
                End If
            End Set
        End Property

        Private _statusBarText As String
        Public Property StatusBarText As String
            Get
                Return _statusBarText
            End Get
            Set(value As String)
                If SetProperty(_statusBarText, value) Then
                    IsStatusBarVisible = True
                End If
            End Set
        End Property

        Private _isStatusBarVisible As Boolean = False
        Public Property IsStatusBarVisible As Boolean
            Get
                Return _isStatusBarVisible
            End Get
            Set(value As Boolean)
                SetProperty(_isStatusBarVisible, value)
            End Set
        End Property

#End Region

#Region " Commands "

        Private _AddShowCommand As ICommand
        Public ReadOnly Property AddShowCommand As ICommand
            Get
                If _AddShowCommand Is Nothing Then
                    _AddShowCommand = New RelayCommand(
                        Sub()
                            My.Application.ShowAddShow()
                        End Sub)
                End If
                Return _AddShowCommand
            End Get
        End Property

        Private _UpcomingEpisodesCommand As ICommand
        Public ReadOnly Property UpcomingEpisodesCommand As ICommand
            Get
                If _UpcomingEpisodesCommand Is Nothing Then
                    _UpcomingEpisodesCommand = New RelayCommand(
                        Sub()
                            My.Application.ShowUpcomingEpisodes()
                        End Sub)
                End If
                Return _UpcomingEpisodesCommand
            End Get
        End Property

        Private _ImportFromFoldersCommand As ICommand
        Public ReadOnly Property ImportFromFoldersCommand As ICommand
            Get
                If _ImportFromFoldersCommand Is Nothing Then
                    _ImportFromFoldersCommand = New RelayCommand(
                        Sub()
                            My.Application.GotoMainWindowState(Application.MainWindowState.ImportFromFolders)
                        End Sub)
                End If
                Return _ImportFromFoldersCommand
            End Get
        End Property

        Private _ConfigureOptionsCommand As ICommand
        Public ReadOnly Property ConfigureOptionsCommand As ICommand
            Get
                If _ConfigureOptionsCommand Is Nothing Then
                    _ConfigureOptionsCommand = New RelayCommand(
                        Sub()
                            My.Application.ShowOptions()
                        End Sub)
                End If
                Return _ConfigureOptionsCommand
            End Get
        End Property

        Private _viewShowCommand As ICommand
        Public ReadOnly Property ViewShowCommand As ICommand
            Get
                If _viewShowCommand Is Nothing Then
                    _viewShowCommand = New RelayCommand(Of TvShow)(
                        Sub(show As TvShow)
                            My.Application.ShowTvShow(show)
                        End Sub)
                End If
                Return _viewShowCommand
            End Get
        End Property

        Private _showEpisodesAvailableNotCollected As ICommand
        Public ReadOnly Property ShowEpisodesAvailableNotCollected As ICommand
            Get
                If _showEpisodesAvailableNotCollected Is Nothing Then
                    _showEpisodesAvailableNotCollected = New RelayCommand(
                    Sub()
                        My.Application.ShowEpisodeList("Episodes Available but Not Collected",
                                                       Function(e) e.IsAvailable = True AndAlso e.IsCollected = False)
                    End Sub)
                End If
                Return _showEpisodesAvailableNotCollected
            End Get
        End Property

        Public ReadOnly Property DeleteTvShowCommand As ICommand
            Get
                Return New RelayCommand(Of TvShow)(
                    Sub(show As TvShow)
                        If MessageWindow.ShowDialog("Are you sure you want to delete the TV show? " &
                                                    Environment.NewLine & "All episode data will be deleted.  This operation cannot be undone.",
                                                    "Really delete?",
                                                    True) = True Then
                            Dim bl As New BLL.BuinessLayer
                            bl.RemoveTvShow(show)
                            RefreshShows()
                        End If
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property UpdateAllShowsCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim th As New Threading.Thread(
                            Sub()
                                If IsUpdatingShows Then
                                    Exit Sub
                                End If
                                IsUpdatingShows = True
                                _isShowsUpdateCancelPending = False
                                Using rg As New RateGate(30, TimeSpan.FromSeconds(10))
                                    For Each show In innerTvShows
                                        If _isShowsUpdateCancelPending Then
                                            InitializeTvShowsCollecionViewSource()
                                            StatusBarText = "Update Cancelled"
                                            _isShowsUpdateCancelPending = False
                                            IsUpdatingShows = False
                                            Exit Sub
                                        End If
                                        rg.WaitToProceed()
                                        StatusBarText = "Updating Shows. Current Show: " & show.Name
                                        show.FullUpdate()
                                        InitializeTvShowsCollecionViewSource()
                                    Next
                                End Using
                                IsUpdatingShows = False
                            End Sub)
                        th.Start()
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property ClearTextFilterCommand As ICommand
            Get
                Return New RelayCommand(Sub()
                                            TextFilter = String.Empty
                                        End Sub)
            End Get
        End Property

        Public ReadOnly Property HideStatusBarCommand As ICommand
            Get
                Return New RelayCommand(Sub()
                                            IsStatusBarVisible = False
                                        End Sub)
            End Get
        End Property

        Private _isShowsUpdateCancelPending As Boolean = False
        Public ReadOnly Property CancelShowsUpdateCommand As ICommand
            Get
                Return New RelayCommand(Sub()
                                            _isShowsUpdateCancelPending = True
                                        End Sub)
            End Get
        End Property

#End Region

        Private Sub TvShowsCollection_Filter(sender As Object, e As FilterEventArgs)
            Dim sho As TvShow = TryCast(e.Item, TvShow)

            If Not String.IsNullOrWhiteSpace(TextFilter) Then
                e.Accepted = sho.Name.IndexOf(TextFilter, StringComparison.OrdinalIgnoreCase) >= 0
                Return
            End If

            If ShowCompleteShows AndAlso Not ShowOnlyFavorites Then
                'show everything
                e.Accepted = True
                Return
            End If

            e.Accepted = If(ShowCompleteShows, True, Not sho.IsCompletelyCollected) And
                         If(ShowOnlyFavorites, sho.IsFavorite, True)
        End Sub

    End Class

End Namespace
