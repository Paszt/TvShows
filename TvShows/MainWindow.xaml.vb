Imports System.ComponentModel

Class MainWindow

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        SetPlacement(My.Settings.MainWindowPlacement)
        If String.IsNullOrWhiteSpace(My.Settings.TvShowRootPath) Then
            My.Application.ShowOptions()
        Else
            My.Application.ShowMainView()
        End If
    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.MainWindowPlacement = Me.GetPlacement()
        My.Settings.Save()
    End Sub

    Private Sub MainWindow_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.Key = Key.Escape Then
            My.Application.ShowMainView()
        End If
    End Sub

End Class
