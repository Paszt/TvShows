Imports System.Collections.ObjectModel
Imports TvShows.Infrastructure

Namespace ViewModels

    Public Class AddShowViewModel
        Inherits Infrastructure.NotifyPropertyChanged

#Region " Properties "

        Private _isConfiguring As Boolean = False
        Public Property IsConfiguring As Boolean
            Get
                Return _isConfiguring
            End Get
            Set(value As Boolean)
                SetProperty(_isConfiguring, value)
                OnPropertyChanged("IsSearching")
            End Set
        End Property

        Public ReadOnly Property IsSearching As Boolean
            Get
                Return Not IsConfiguring
            End Get
        End Property

        Private _IsWorking As Boolean
        Public Property IsWorking As Boolean
            Get
                Return _IsWorking
            End Get
            Set(value As Boolean)
                SetProperty(_IsWorking, value)
            End Set
        End Property

#Region " Searching Properties "

        Private _queryString As String
        Public Property QueryString As String
            Get
                Return _queryString
            End Get
            Set(value As String)
                SetProperty(_queryString, value)
            End Set
        End Property

        Private _searchResults As List(Of Model.TvShow)
        Public Property SearchResults As List(Of Model.TvShow)
            Get
                Return _searchResults
            End Get
            Set(value As List(Of Model.TvShow))
                SetProperty(_searchResults, value)
            End Set
        End Property

        Private _selectedTvShow As Model.TvShow
        Public Property SelectedTvShow As Model.TvShow
            Get
                Return _selectedTvShow
            End Get
            Set(value As Model.TvShow)
                SetProperty(_selectedTvShow, value)
                If value Is Nothing Then
                    ShowFolderName = Nothing
                Else
                    ShowFolderName = value.Name
                End If

            End Set
        End Property

#End Region

#Region " Configuring Properties "

        Public Property ShowFolderName As String
            Get
                If SelectedTvShow Is Nothing Then
                    Return Nothing
                Else
                    Return SelectedTvShow.Directory
                End If
            End Get
            Set(value As String)
                If SelectedTvShow IsNot Nothing Then
                    SelectedTvShow.Directory = value.Trim("\"c)
                End If
                OnPropertyChanged("ShowFolderName")
            End Set
        End Property

#End Region

#End Region

#Region " Commands "

        Public ReadOnly Property AddShowCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim th As New Threading.Thread(
                            Sub()
                                IsWorking = True
                                Dim fullTvShow As Model.TvShow = Tmdb.Api.GetTvShowWithEpisodes(CInt(SelectedTvShow.Id))
                                fullTvShow.Directory = SelectedTvShow.Directory
                                fullTvShow.UpdateEpisodeFilePaths(False)
                                Dim bl As New BLL.BuinessLayer
                                Dim result = bl.AddTvShow(fullTvShow)
                                If result.Succeeded = False Then
                                    MessageWindow.ShowDialog(result.Message, "Error adding TV Show")
                                End If
                                IsWorking = False
                                My.Application.ShowMainView()
                            End Sub)
                        th.Start()
                    End Sub,
                    Function() As Boolean
                        Return SelectedTvShow IsNot Nothing AndAlso Not String.IsNullOrEmpty(SelectedTvShow.Directory)
                    End Function)
            End Get
        End Property

#Region " Navigation Commands "

        Public ReadOnly Property CancelCommand() As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        My.Application.ShowMainView()
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property MoveNextCommand() As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        IsConfiguring = True
                    End Sub,
                    Function() As Boolean
                        Return SelectedTvShow IsNot Nothing AndAlso SelectedTvShow.Id.HasValue
                    End Function)
            End Get
        End Property

        Public ReadOnly Property MovePreviousCommand() As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        IsConfiguring = False
                    End Sub,
                    Function() As Boolean
                        Return IsConfiguring = True
                    End Function)
            End Get
        End Property

#End Region

#Region " Searching Commands "

        Private _searchCommand As ICommand
        Public ReadOnly Property SearchCommand As ICommand
            Get
                If _searchCommand Is Nothing Then
                    _searchCommand = New RelayCommand(AddressOf ExecuteSearch, AddressOf CanExecuteSearch)
                End If
                Return _searchCommand
            End Get
        End Property

        Private Sub ExecuteSearch()
            SearchResults = Tmdb.Api.TvShowSearch(QueryString)
        End Sub

        Private Function CanExecuteSearch() As Boolean
            Return Not String.IsNullOrWhiteSpace(QueryString)
        End Function

#End Region

#Region " Configuring Commands "

        Public ReadOnly Property BrowseForFolderCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim ofd As New Forms.FolderBrowserDialog() With {
                        .SelectedPath = My.Settings.TvShowRootPath
                        }

                        If ofd.ShowDialog = Forms.DialogResult.OK Then
                            'If ofd.SelectedPath.StartsWith(My.Settings.TvShowRootPath) Then
                            'ShowFolderName = ofd.SelectedPath.Replace(My.Settings.TvShowRootPath, String.Empty).Trim("\"c)
                            'Else
                            ShowFolderName = ofd.SelectedPath
                            'End If
                        End If
                    End Sub)
            End Get
        End Property

#End Region

#End Region

    End Class

End Namespace
