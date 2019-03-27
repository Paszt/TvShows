Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Runtime.Serialization

Namespace Model

    <DataContract>
    Partial Public Class Episode
        Inherits Infrastructure.NotifyPropertyChanged
        Implements IEntity

        Public Shared Function NewFromRemote(epi As Episode, show As TvShow) As Episode
            Dim ep As New Episode() With {
                .AirDateString = epi.AirDateString,
                .EpisodeNumber = epi.EpisodeNumber,
                .Id = epi.Id,
                .Name = epi.Name,
                .Overview = epi.Overview,
                .SeasonNumber = epi.SeasonNumber,
                .StillPath = epi.StillPath,
                .TvShow = show,
                .EntityState = EntityState.Added
            }
            Return ep
        End Function

        <DataMember(Name:="id")>
        Public Property Id As Integer? Implements IEntity.Id

        Private _AirDateString As String
        <DataMember(Name:="air_date"), StringLength(10)>
        Public Property AirDateString As String
            Get
                Return _AirDateString
            End Get
            Set(value As String)
                If SetProperty(_AirDateString, value) = True Then
                    OnPropertyChanged("AirDate")
                    OnPropertyChanged("IsAvailable")
                    OnPropertyChanged("IsNotAvailable")
                End If
            End Set
        End Property

        <NotMapped>
        Public ReadOnly Property AirDate As Date?
            Get
                Dim dte As Date
                If Date.TryParse(AirDateString, dte) Then
                    Return dte
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property AirDateLong As String
            Get
                If AirDate.HasValue Then
                    Return AirDate.Value.ToLongDateString
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        <DataMember(Name:="season_number")>
        Public Property SeasonNumber As Integer

        <DataMember(Name:="episode_number")>
        Public Property EpisodeNumber As Integer

        Private _Name As String
        <DataMember(Name:="name")>
        Public Property Name As String Implements IEntity.Name
            Get
                Return _Name
            End Get
            Set(value As String)
                SetProperty(_Name, value)
            End Set
        End Property

        Private _Overview As String
        <DataMember(Name:="overview")>
        Public Property Overview As String
            Get
                Return _Overview
            End Get
            Set(value As String)
                If SetProperty(_Overview, value) = True Then
                    OnPropertyChanged("HasOverview")
                End If
            End Set
        End Property

        <NotMapped>
        Public ReadOnly Property HasOverview As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(Overview)
            End Get
        End Property

        Private _StillPath As String
        <DataMember(Name:="still_path")>
        Public Property StillPath As String
            Get
                Return _StillPath
            End Get
            Set(value As String)
                SetProperty(_StillPath, value)
            End Set
        End Property

        Public Overridable Property TvShow As TvShow

        Public Property TvShowId As Integer

        <NotMapped>
        Public Property EntityState As EntityState Implements IEntity.EntityState

        Private _FilePath As String
        Public Property FilePath As String
            Get
                Return _FilePath
            End Get
            Set(value As String)
                If Not String.IsNullOrWhiteSpace(_FilePath) AndAlso String.IsNullOrWhiteSpace(value) Then
                    IsArchived = True
                End If
                If Not String.IsNullOrWhiteSpace(value) Then
                    IsArchived = False
                End If
                If SetProperty(_FilePath, value) = True Then
                    EntityState = EntityState.Modified
                    OnPropertyChanged("FileName")
                    OnPropertyChanged("FilePathExists")
                    OnPropertyChanged("FullFilePath")
                    OnPropertyChanged("IsArchived")
                    OnPropertyChanged("IsCollected")
                    OnPropertyChanged("IsNotCollected")
                    OnPropertyChanged("HasFilePath")
                End If
            End Set
        End Property

        Public ReadOnly Property HasFilePath As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(FilePath)
            End Get
        End Property

        Public ReadOnly Property FullFilePath As String
            Get
                If String.IsNullOrWhiteSpace(FilePath) OrElse Uri.IsWellFormedUriString(FilePath, UriKind.Absolute) Then
                    Return FilePath
                Else
                    Return IO.Path.Combine(My.Settings.TvShowRootPath, FilePath.TrimStart(New Char() {"\"c, "/"c}))
                End If
            End Get
        End Property

        Public ReadOnly Property FilePathExists As Boolean
            Get
                Return IO.File.Exists(FullFilePath)
            End Get
        End Property

        Public ReadOnly Property FileName As String
            Get
                Return IO.Path.GetFileName(FilePath)
            End Get
        End Property

        Public ReadOnly Property SeasonEpisodeString As String
            Get
                Return "S" & SeasonNumber.ToString("00") & "E" & EpisodeNumber.ToString("00")
            End Get
        End Property

        Private _IsArchived As Boolean
        Public Property IsArchived As Boolean
            Get
                Return _IsArchived
            End Get
            Set(value As Boolean)
                ' don't allow unavailable episodes to be archived
                If value = True AndAlso IsNotAvailable Then
                    Exit Property
                End If
                If SetProperty(_IsArchived, value) = True Then
                    OnPropertyChanged("IsCollected")
                    OnPropertyChanged("IsNotCollected")
                End If
            End Set
        End Property

        Public ReadOnly Property IsAvailable As Boolean
            Get
                Return AirDate.HasValue AndAlso AirDate.Value <= Date.Now()
            End Get
        End Property

        Public ReadOnly Property IsNotAvailable As Boolean
            Get
                Return Not IsAvailable
            End Get
        End Property

        Public ReadOnly Property IsCollected As Boolean
            Get
                Return Not String.IsNullOrEmpty(FilePath) Or IsArchived
            End Get
        End Property

        Public ReadOnly Property IsNotCollected As Boolean
            Get
                Return Not IsCollected
            End Get
        End Property

        Private _IsSelected As Boolean
        <NotMapped>
        Public Property IsSelected As Boolean
            Get
                Return _IsSelected
            End Get
            Set(value As Boolean)
                SetProperty(_IsSelected, value)
            End Set
        End Property

        Public ReadOnly Property GotoTmdbPageCommand() As ICommand
            Get
                Return New Infrastructure.RelayCommand(
                    Sub()
                        Process.Start("https://www.themoviedb.org/tv/" & TvShowId & "/season/" & SeasonNumber & "/episode/" & EpisodeNumber)
                    End Sub)
            End Get
        End Property

        <NotMapped>
        Public ReadOnly Property ShowEpisodeImagesInverted As Boolean
            Get
                Return Not My.Settings.ShowEpisodeImages
            End Get
        End Property

        Private WithEvents Mysettings As MySettings = My.Settings
        Private Sub Mysettings_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Mysettings.PropertyChanged
            If e.PropertyName = "ShowEpisodeImages" Then
                OnPropertyChanged("ShowEpisodeImagesInverted")
            End If
        End Sub

        'Public Function BaseInfoEquals(ep As Episode) As Boolean
        '    Return ep.AirDate = AirDate AndAlso ep.Name = Name AndAlso ep.Overview = Overview
        'End Function

        Public Sub Update(ep As Episode)
            If AirDateString <> ep.AirDateString Then
                AirDateString = ep.AirDateString
                EntityState = EntityState.Modified
            End If
            If Name <> ep.Name Then
                Name = ep.Name
                EntityState = EntityState.Modified
            End If
            If Overview <> ep.Overview Then
                Overview = ep.Overview
                EntityState = EntityState.Modified
            End If
            If StillPath <> ep.StillPath Then
                StillPath = ep.StillPath
                EntityState = EntityState.Modified
            End If
            'AirDate = ep.AirDate
            'Name = ep.Name
            'Overview = ep.Overview
        End Sub

        Public Overrides Function ToString() As String
            Dim returnString = String.Empty
            If TvShow IsNot Nothing Then
                returnString = TvShow.Name & " "
            End If
            returnString &= SeasonEpisodeString
            If Not String.IsNullOrWhiteSpace(Name) Then
                returnString &= " - " & Name
            End If
            Return returnString.Trim()
        End Function

    End Class

End Namespace
