Imports TvShows.Infrastructure

Namespace ViewModels

    Public Class OptionsViewModel

        Private _TmdbConfigUpdated As Boolean = False

        Private _BrowseForDownloadFolderCommand As ICommand
        Public ReadOnly Property BrowseForDownloadFolderCommand As ICommand
            Get
                If _BrowseForDownloadFolderCommand Is Nothing Then
                    _BrowseForDownloadFolderCommand = New RelayCommand(
                        Sub()
                            Dim ofd As New Forms.FolderBrowserDialog()
                            If ofd.ShowDialog = Forms.DialogResult.OK Then
                                My.Settings.TvShowRootPath = ofd.SelectedPath
                            End If
                        End Sub)
                End If
                Return _BrowseForDownloadFolderCommand
            End Get
        End Property

        Private _OkCommand As ICommand
        Public ReadOnly Property OkCommand As ICommand
            Get
                If _OkCommand Is Nothing Then
                    _OkCommand = New RelayCommand(AddressOf ExecuteOk, AddressOf CanExecuteOk)
                End If
                Return _OkCommand
            End Get
        End Property

        Private Sub ExecuteOk()
            If Not IO.Directory.Exists(My.Settings.TvShowRootPath) Then
                If MessageWindow.ShowDialog(My.Settings.TvShowRootPath & " does not exist. Do you want to create it?", "Create new folder?", True) = False Then
                    Exit Sub
                Else
                    IO.Directory.CreateDirectory(My.Settings.TvShowRootPath)
                End If
            End If
            My.Settings.Save()
            My.Application.ShowMainView()
        End Sub

        Private Function CanExecuteOk() As Boolean
            Return Not String.IsNullOrWhiteSpace(My.Settings.TvShowRootPath)
        End Function

        Private _UpdateTmdbConfigCommand As ICommand
        Public ReadOnly Property UpdateTmdbConfigCommand As ICommand
            Get
                If _UpdateTmdbConfigCommand Is Nothing Then
                    _UpdateTmdbConfigCommand = New RelayCommand(
                        Sub()
                            Tmdb.Api.GetConfiguration(forceUpdate:=True)
                            _TmdbConfigUpdated = True
                        End Sub,
                        Function() As Boolean
                            Return _TmdbConfigUpdated = False
                        End Function)
                End If
                Return _UpdateTmdbConfigCommand
            End Get
        End Property

    End Class

End Namespace
