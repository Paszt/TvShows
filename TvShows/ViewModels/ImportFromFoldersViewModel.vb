Namespace ViewModels

    Public Class ImportFromFoldersViewModel
        Inherits Infrastructure.NotifyPropertyChanged

        Public Sub New()
            If Not ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
                Dim th As New Threading.Thread(AddressOf ImportExecute)
                th.Start()
            End If
        End Sub

#Region " Properties "

        Private _IsWorking As Boolean = True
        Public Property IsWorking As Boolean
            Get
                Return _IsWorking
            End Get
            Set(value As Boolean)
                SetProperty(_IsWorking, value)
                OnPropertyChanged("IsImportComplete")
            End Set
        End Property

        Public ReadOnly Property IsImportComplete As Boolean
            Get
                Return Not IsWorking
            End Get
        End Property

        Private _TotalFolderCount As Integer
        Public Property TotalFolderCount As Integer
            Get
                Return _TotalFolderCount
            End Get
            Set(value As Integer)
                SetProperty(_TotalFolderCount, value)
                OnProgressChanged()
            End Set
        End Property

        Private _FolderCounter As Integer
        Public Property FolderCounter As Integer
            Get
                Return _FolderCounter
            End Get
            Set(value As Integer)
                SetProperty(_FolderCounter, value)
                OnProgressChanged()
            End Set
        End Property

        Public ReadOnly Property Progress As Integer
            Get
                If FolderCounter = 0 Then
                    Return 0
                End If
                Return CInt((TotalFolderCount / FolderCounter) * 100)
            End Get
        End Property

        Private _ResultMessage As String
        Public Property ResultMessage As String
            Get
                Return _ResultMessage
            End Get
            Set(value As String)
                SetProperty(_ResultMessage, value)
            End Set
        End Property

#End Region

        Private Sub OnProgressChanged()
            OnPropertyChanged("Progress")
        End Sub

        Private Sub ImportExecute()
            '''
            Exit Sub '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            FolderCounter = 0
            Dim bl As New BLL.BuinessLayer
            Dim existingTvShows As List(Of Model.TvShow) = bl.GetAllTvShows().ToList()
            Dim folderPaths = IO.Directory.GetDirectories(My.Settings.TvShowRootPath)
            For Each path In folderPaths
                Dim showName = New IO.DirectoryInfo(path).Name
                If existingTvShows.Where(Function(t) t.FullDirectoryPath = path).Count > 0 Then
                    AddResultMessage(showName, "already added.")
                    Continue For
                End If

                Dim newShow = Tmdb.Api.TvShowSearch(showName).FirstOrDefault()
                If newShow Is Nothing Then
                    AddResultMessage(showName, "no TV show found for this name. Skipped import.")
                    Continue For
                End If
                If newShow.Name = showName Then
                    Dim fullTvShow = Tmdb.Api.GetTvShowWithEpisodes(CInt(newShow.Id))
                    fullTvShow.Directory = path
                    fullTvShow.UpdateEpisodeFilePaths(False)
                    Dim result = bl.AddTvShow(fullTvShow)
                    If result.Succeeded = False Then
                        AddResultMessage(showName, "Error while adding. " & result.Message)
                    Else
                        AddResultMessage(showName, "added successfully.")
                        existingTvShows.Add(fullTvShow)
                    End If
                Else
                    AddResultMessage(showName, "results found but they were not an exact match. Skipped import.")
                End If
                FolderCounter += 1
            Next
        End Sub

        Private Sub AddResultMessage(showName As String, message As String)
            ResultMessage &= showName & ": " & message & Environment.NewLine
        End Sub

    End Class

End Namespace
