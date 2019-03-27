Imports System.ComponentModel
Imports TvShows.Infrastructure
Imports TvShows.Model

Namespace ViewModels

    Public Class ShowViewModel
        Inherits NotifyPropertyChanged

        Dim timr As Threading.Timer

        Public Sub New(show As TvShow)
            TvShow = show
            InitializeEpisodesCollectionView()
            timr = New Threading.Timer(AddressOf CheckForSelectedEpisodes, Nothing, 20, 20)
        End Sub

        Private Sub InitializeEpisodesCollectionView()
            EpisodesCollectionView = New ListCollectionView(CType(TvShow.Episodes, IList))
            EpisodesCollectionView.GroupDescriptions.Add(New PropertyGroupDescription("SeasonNumber"))
            'EpisodesCollectionView.SortDescriptions.Add(New SortDescription("SeasonNumber", ListSortDirection.Ascending))
            'EpisodesCollectionView.SortDescriptions.Add(New SortDescription("EpisodeNumber", ListSortDirection.Ascending))
            EpisodesCollectionView.CustomSort = New EpisodeSorter()
        End Sub

        Private Sub CheckForSelectedEpisodes(state As Object)
            If TvShow IsNot Nothing AndAlso TvShow.Episodes IsNot Nothing Then
                IsEpisodeSelected = (TvShow.Episodes.Where(Function(e) e.IsSelected = True).Count > 0)
            Else
                IsEpisodeSelected = False
            End If
        End Sub

#Region " Properties "

        Private _IsEpisodeSelected As Boolean
        Public Property IsEpisodeSelected As Boolean
            Get
                'Return False
                Return _IsEpisodeSelected
            End Get
            Set(value As Boolean)
                SetProperty(_IsEpisodeSelected, value)
            End Set
        End Property

        Private _TvShow As TvShow
        Public Property TvShow As TvShow
            Get
                Return _TvShow
            End Get
            Set(value As TvShow)
                SetProperty(_TvShow, value)
            End Set
        End Property

        Private _EpisodesCollectionView As ListCollectionView
        Public Property EpisodesCollectionView As ListCollectionView
            Get
                Return _EpisodesCollectionView
            End Get
            Set(value As ListCollectionView)
                SetProperty(_EpisodesCollectionView, value)
            End Set
        End Property

#End Region

#Region " Commands "

        Public ReadOnly Property SeasonCheckBoxCheckedCommand As ICommand
            Get
                Return New RelayCommand(Of Object)(
                    Sub(checkbox As Object)
                        Dim cb As CheckBox = CType(checkbox, CheckBox)
                        Dim groupObjects As ObjectModel.ReadOnlyObservableCollection(Of Object) = CType(cb.Tag, CollectionViewGroup).Items

                        For Each obj In groupObjects
                            Dim ep As Model.Episode = CType(obj, Episode)
                            ep.IsSelected = cb.IsChecked.Value
                        Next

                    End Sub)
            End Get
        End Property

        Public ReadOnly Property BrowseForDirectoryCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim ofd As New Forms.FolderBrowserDialog()
                        If IO.Directory.Exists(TvShow.FullDirectoryPath) Then
                            ofd.SelectedPath = TvShow.FullDirectoryPath
                        Else
                            ofd.SelectedPath = My.Settings.TvShowRootPath
                        End If
                        If ofd.ShowDialog = Forms.DialogResult.OK Then
                            Dim newDirectoryValue As String
                            If ofd.SelectedPath.StartsWith(My.Settings.TvShowRootPath) Then
                                newDirectoryValue = ofd.SelectedPath.Replace(My.Settings.TvShowRootPath, String.Empty).Trim("\"c)
                            Else
                                newDirectoryValue = ofd.SelectedPath
                            End If
                            If newDirectoryValue <> TvShow.Directory Then
                                TvShow.Directory = newDirectoryValue
                                TvShow.UpdateEpisodeFilePaths()
                            End If
                        End If
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property SetEpisodesArchivedCommand As ICommand
            Get
                Return New RelayCommand(Of Object)(
                    Sub(comboboxObj As Object)
                        Dim bl As New BLL.BuinessLayer
                        Dim combo As ComboBox = CType(comboboxObj, ComboBox)
                        Dim archivedValue As Boolean = False
                        If CType(combo.SelectedValue, ComboBoxItem).Content.ToString = "Archived" Then
                            archivedValue = True
                        End If
                        For Each ep In TvShow.Episodes.Where(Function(e) e.IsSelected = True)
                            If ep.IsArchived <> archivedValue Then
                                ep.IsArchived = archivedValue
                                ep.EntityState = EntityState.Modified
                            End If
                            ep.IsSelected = False
                            'bl.UpdateEpisode(ep)
                        Next
                        Dim result = bl.UpdateTvShow(TvShow)
                        If Not result.Succeeded Then
                            MessageWindow.ShowDialog(result.Message, "Error saving TV Show")
                        End If
                    End Sub,
                    Function(comboBoxObj As Object) As Boolean
                        Dim combo As ComboBox = CType(comboBoxObj, ComboBox)
                        Return combo.SelectedValue IsNot Nothing
                    End Function)
            End Get
        End Property

        Public ReadOnly Property SelectAllEpisodesCommand As ICommand
            Get
                Return New RelayCommand(Of Object)(
                    Sub(checkbox As Object)
                        Dim cb As CheckBox = CType(checkbox, CheckBox)

                        For Each ep In TvShow.Episodes
                            ep.IsSelected = cb.IsChecked.Value
                        Next
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property FullUpdateCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim result = TvShow.FullUpdate()
                        InitializeEpisodesCollectionView()
                        If String.IsNullOrWhiteSpace(result.Message) Then
                            result.Message = "Update succeeded with no messages."
                        End If
                        MessageWindow.ShowDialog(result.Message, TvShow.Name & " - Full Episode Update Results")
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property UpdateEpisodePathsCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        Dim result = TvShow.UpdateEpisodeFilePaths()
                        If String.IsNullOrWhiteSpace(result.Message) Then
                            result.Message = "Update succeeded with no messages."
                        End If
                        MessageWindow.ShowDialog(result.Message, TvShow.Name & " - Episode Path Update Results")
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property DeleteEpisodeByIdCommand As ICommand
            Get
                Return New RelayCommand(Of Episode)(
                    Sub(epId As Episode)
                        Dim bl As New BLL.BuinessLayer
                        bl.RemoveEpisode(epId)
                        InitializeEpisodesCollectionView()
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property CloseCommand As ICommand
            Get
                Return New RelayCommand(
                    Sub()
                        My.Application.ShowMainView()
                    End Sub)
            End Get
        End Property

#End Region

    End Class

End Namespace
