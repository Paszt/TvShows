Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Runtime.Serialization

Namespace Model

    <DataContract>
    Partial Public Class TvShow
        Inherits Infrastructure.NotifyPropertyChanged
        Implements IEntity

        <DataMember(Name:="id"), DatabaseGenerated(DatabaseGeneratedOption.None), Required>
        Public Property Id As Integer? Implements IEntity.Id

        <DataMember(Name:="name"), MaxLength(255), Required>
        Public Property Name As String Implements IEntity.Name

        <DataMember(Name:="first_air_date"), StringLength(10)>
        Public Property FirstAirDate As String

        <NotMapped>
        Public ReadOnly Property Year As String
            Get
                If FirstAirDate IsNot Nothing AndAlso FirstAirDate.Length > 4 Then
                    Return "(" & FirstAirDate.Substring(0, 4) & ")"
                End If
                Return Nothing
            End Get
        End Property

        <DataMember(Name:="homepage"), MaxLength(1000)>
        Public Property HomePage As String

        <StringLength(2)>
        Public Property OriginCountry As String
            Get
                If OriginCountries IsNot Nothing AndAlso OriginCountries.Count > 0 Then
                    Return OriginCountries(0)
                Else
                    Return Nothing
                End If
            End Get
            Set(value As String)

            End Set
        End Property

        <DataMember(Name:="origin_country"), NotMapped>
        Public Property OriginCountries As List(Of String)

        <DataMember(Name:="overview")>
        Public Property Overview As String

        <NotMapped>
        Public ReadOnly Property OverviewSnip As String
            Get
                If String.IsNullOrWhiteSpace(Overview) Then
                    Return Overview
                Else
                    Dim ret = Overview
                    If Overview.Length > 300 Then
                        ret = Overview.Substring(0, 300) & " ..."
                    End If
                    Return ret
                End If
            End Get
        End Property

        Private _BackdropPath As String
        <DataMember(Name:="backdrop_path")>
        Public Property BackdropPath As String
            Get
                Return _BackdropPath
            End Get
            Set(value As String)
                SetProperty(_BackdropPath, value)
            End Set
        End Property

        Private _PosterPath As String
        <DataMember(Name:="poster_path")>
        Public Property PosterPath As String
            Get
                Return _PosterPath
            End Get
            Set(value As String)
                SetProperty(_PosterPath, value)
            End Set
        End Property

        Private _Directory As String
        ''' <summary>
        ''' The directory path where the episode files are stored. Can be absolute or relative to the TvShowRootPath setting.
        ''' </summary>
        Public Property Directory As String
            Get
                Return _Directory
            End Get
            Set(value As String)
                'make directories in the TvShowRootPath relative
                If value.StartsWith(My.Settings.TvShowRootPath) Then
                    value = value.Replace(My.Settings.TvShowRootPath, String.Empty).Trim("\"c)
                End If
                SetProperty(_Directory, value)
                OnPropertyChanged("FullDirectoryPath")
                OnPropertyChanged("DirectoryExists")
            End Set
        End Property

        <NotMapped>
        Public ReadOnly Property FullDirectoryPath As String
            Get
                If String.IsNullOrWhiteSpace(Directory) Then
                    Return Nothing
                End If
                If Uri.IsWellFormedUriString(Directory, UriKind.Absolute) Then
                    Return Directory
                Else
                    Return IO.Path.Combine(My.Settings.TvShowRootPath, Directory)
                End If
            End Get
        End Property

        <NotMapped>
        Public ReadOnly Property DirectoryExists As Boolean
            Get
                If String.IsNullOrWhiteSpace(FullDirectoryPath) Then
                    Return False
                End If
                Return IO.Directory.Exists(FullDirectoryPath)
            End Get
        End Property

        Public Overridable Property Episodes As IList(Of Episode) = New List(Of Episode)

        <NotMapped>
        Public Property EntityState As EntityState Implements IEntity.EntityState

        <NotMapped>
        Public ReadOnly Property EpisodesCollectedCount As Integer
            Get
                If Episodes Is Nothing OrElse Episodes.Count = 0 Then
                    Return 0
                End If
                Return Episodes.Where(Function(e) e.IsCollected = True AndAlso e.SeasonNumber <> 0).Count
            End Get
        End Property

        <NotMapped>
        Public ReadOnly Property EpisodesAvailableCount As Integer
            Get
                If Episodes Is Nothing OrElse Episodes.Count = 0 Then
                    Return 0
                End If
                Return Episodes.Where(Function(e) e.IsAvailable = True AndAlso e.SeasonNumber <> 0).Count
            End Get
        End Property

        <NotMapped>
        Public ReadOnly Property Progress As Integer
            Get
                If EpisodesAvailableCount = 0 Then
                    Return 100
                Else
                    Return CInt((EpisodesCollectedCount / EpisodesAvailableCount) * 100)
                End If
            End Get
        End Property

        <NotMapped>
        Public ReadOnly Property IsCompletelyCollected As Boolean
            Get
                Return Progress = 100
            End Get
        End Property

        Private _IsFavorite As Boolean
        Public Property IsFavorite As Boolean
            Get
                Return _IsFavorite
            End Get
            Set(value As Boolean)
                SetProperty(_IsFavorite, value)
            End Set
        End Property

        Public ReadOnly Property ToggleIsFavorite() As ICommand
            Get
                Return New Infrastructure.RelayCommand(
                    Sub()
                        IsFavorite = Not IsFavorite
                        Me.EntityState = EntityState.Modified
                        Dim bl As New BLL.BuinessLayer()
                        bl.UpdateTvShow(Me)
                    End Sub)
            End Get
        End Property

        <DataMember(Name:="seasons"), NotMapped>
        Public Property Seasons As Season()

        <DataContract, NotMapped>
        Public Class Season

            <DataMember(Name:="air_date")>
            Public Property AirDate As String

            <DataMember(Name:="episode_count")>
            Public Property EpisodeCount As Integer

            <DataMember(Name:="id")>
            Public Property Id As Integer

            <DataMember(Name:="poster_path")>
            Public Property PosterPath As String

            <DataMember(Name:="season_number")>
            Public Property SeasonNumber As Integer

        End Class

        <DataMember(Name:="external_ids")>
        Public Property ExternalIds As TvShowExternalIds

        <DataContract>
        Public Class TvShowExternalIds

            <DataMember(Name:="imdb_id")>
            Public Property ImdbId As String

            <DataMember(Name:="freebase_mid")>
            Public Property FreebaseMid As String

            <DataMember(Name:="freebase_id")>
            Public Property FreebaseId As String

            <DataMember(Name:="tvdb_id")>
            Public Property TvdbId As Integer?

            <DataMember(Name:="tvrage_id")>
            Public Property TvrageId As Integer?

        End Class

        Public Shared Operator =(t1 As TvShow, t2 As TvShow) As Boolean
            Return t1.Directory = t2.Directory AndAlso
                t1.HomePage = t2.HomePage AndAlso
                t1.Id.Equals(t2.Id) AndAlso
                t1.IsFavorite = t2.IsFavorite AndAlso
                t1.Name = t2.Name AndAlso
                t1.OriginCountry = t2.OriginCountry AndAlso
                t1.Overview = t2.Overview AndAlso
                t1.PosterPath = t2.PosterPath AndAlso
                t1.BackdropPath = t2.BackdropPath
        End Operator

        Public Shared Operator <>(t1 As TvShow, t2 As TvShow) As Boolean
            Return t1.Directory <> t2.Directory OrElse
                t1.HomePage <> t2.HomePage OrElse
                Not t1.Id.Equals(t2.Id) OrElse
                t1.IsFavorite <> t2.IsFavorite OrElse
                t1.Name <> t2.Name OrElse
                t1.OriginCountry <> t2.OriginCountry OrElse
                t1.Overview <> t2.Overview OrElse
                t1.PosterPath <> t2.PosterPath OrElse
             t1.BackdropPath <> t2.BackdropPath
        End Operator

#Region " Commands "

        Public ReadOnly Property GotoImdbPageCommand() As ICommand
            Get
                Return New Infrastructure.RelayCommand(
                    Sub()
                        Process.Start("http://www.imdb.com/title/" & ExternalIds.ImdbId)
                    End Sub,
                    Function() As Boolean
                        Return Not String.IsNullOrWhiteSpace(ExternalIds.ImdbId)
                    End Function)
            End Get
        End Property

        Public ReadOnly Property GotoTmdbPageCommand() As ICommand
            Get
                Return New Infrastructure.RelayCommand(
                    Sub()
                        Process.Start("https://www.themoviedb.org/tv/" & id)
                    End Sub)
            End Get
        End Property

        Public ReadOnly Property GotoLocalFolderCommand As ICommand
            Get
                Return New Infrastructure.RelayCommand(
                    Sub()
                        Process.Start(FullDirectoryPath)
                    End Sub,
                    Function() As Boolean
                        Return IO.Directory.Exists(FullDirectoryPath)
                    End Function)
            End Get
        End Property

#End Region

    End Class

End Namespace
