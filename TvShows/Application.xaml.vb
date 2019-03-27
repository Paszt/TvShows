Option Strict On

Imports System.ComponentModel

Class Application

    Private showListModel As ViewModels.ShowListViewModel

    Public rg As Infrastructure.RateGate

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        rg = New Infrastructure.RateGate(38, TimeSpan.FromSeconds(10))
        showListModel = New ViewModels.ShowListViewModel()
    End Sub

    'Private Sub ShowMatches(r As Text.RegularExpressions.Regex, m As Text.RegularExpressions.Match)
    '    Dim names() As String = r.GetGroupNames()
    '    Console.WriteLine("Named Groups:")
    '    For Each name In names
    '        Dim grp As Text.RegularExpressions.Group = m.Groups.Item(name)
    '        Console.WriteLine("   {0}: '{1}'", name, grp.Value)
    '    Next
    'End Sub

    Private Function TheMainWindow() As MainWindow
        Return CType(MainWindow, MainWindow)
    End Function

    Public Enum MainWindowState
        ShowList
        AddShow
        ViewShow
        ImportFromFolders
        EpisodeList
        Options
        UpcomingEpisodes
    End Enum

    Public Delegate Sub GotoMainWindowStateDelegate(state As MainWindowState, show As Model.TvShow)

    Public Sub GotoMainWindowState(state As MainWindowState, Optional show As Model.TvShow = Nothing)
        If Dispatcher.CheckAccess() Then
            Select Case state
                Case MainWindowState.ShowList
                    If TheMainWindow.MainContentPresenter.Content Is Nothing OrElse
                        Not TheMainWindow.MainContentPresenter.Content.GetType Is GetType(ViewModels.ShowListViewModel) Then
                        TheMainWindow.MainContentPresenter.Content = showListModel
                        showListModel.RefreshShows()
                    End If
                Case MainWindowState.AddShow
                    TheMainWindow.MainContentPresenter.Content = New ViewModels.AddShowViewModel()
                Case MainWindowState.ViewShow
                    TheMainWindow.MainContentPresenter.Content = New ViewModels.ShowViewModel(show)
                Case MainWindowState.ImportFromFolders
                    TheMainWindow.MainContentPresenter.Content = New ViewModels.ImportFromFoldersViewModel()
                Case MainWindowState.Options
                    TheMainWindow.MainContentPresenter.Content = New ViewModels.OptionsViewModel()
                Case MainWindowState.UpcomingEpisodes
                    TheMainWindow.MainContentPresenter.Content = New ViewModels.UpcomingEpisodesViewModel()
                Case MainWindowState.EpisodeList
                    'TODO: Episode List Window State
                    Exit Select
                Case Else
                    Exit Select
            End Select
        Else
            Dispatcher.Invoke(New GotoMainWindowStateDelegate(AddressOf GotoMainWindowState), New Object() {state, show})
        End If
    End Sub

    Public Sub ShowMainView()
        GotoMainWindowState(MainWindowState.ShowList)
    End Sub

    Public Sub ShowAddShow()
        GotoMainWindowState(MainWindowState.AddShow)
    End Sub

    Public Sub ShowOptions()
        GotoMainWindowState(MainWindowState.Options)
    End Sub

    Public Sub ShowUpcomingEpisodes()
        GotoMainWindowState(MainWindowState.UpcomingEpisodes)
    End Sub

    Public Sub ShowTvShow(show As Model.TvShow)
        GotoMainWindowState(MainWindowState.ViewShow, show)
    End Sub

    Public Delegate Sub ShowEpisodeListDelegate(viewTitle As String, predicate As Func(Of Model.Episode, Boolean))

    Public Sub ShowEpisodeList(viewTitle As String, predicate As Func(Of Model.Episode, Boolean))
        If Dispatcher.CheckAccess() Then
            TheMainWindow.MainContentPresenter.Content = New ViewModels.EpisodeListViewModel(viewTitle, predicate)
        Else
            Dispatcher.Invoke(New ShowEpisodeListDelegate(AddressOf ShowEpisodeList), New Object() {viewTitle, predicate})
        End If
    End Sub

    Private _tmdbConfig As Tmdb.Configuration
    Public ReadOnly Property TmdbImageBaseUrl As String
        Get
            If _tmdbConfig Is Nothing Then
                _tmdbConfig = Tmdb.Api.GetConfiguration()
            End If
            Return _tmdbConfig.Image.BaseUrl
        End Get
    End Property

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs) Handles Me.Exit
        rg.Dispose()
    End Sub

End Class
