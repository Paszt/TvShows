Imports System.ComponentModel
Imports TvShows.Model

Namespace ViewModels

    Public Class UpcomingEpisodesViewModel
        Inherits Infrastructure.NotifyPropertyChanged

        Private innerEpisodes As List(Of Episode)
        Private EpisodesCollectionViewSource As CollectionViewSource

        Public Sub New()
            Dim th As New Threading.Thread(
                Sub()
                    Dim bl As New BLL.BuinessLayer
                    If Not DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
                        innerEpisodes = bl.GetAllEpisodes().OrderBy(Function(e) e.SeasonNumber).ThenBy(Function(e) e.EpisodeNumber).ToList()
                        InitializeEpisodesCollecionViewSource()
                    End If
                End Sub)
            th.Start()
        End Sub

#Region " Properties "

        Public ReadOnly Property EpisodesCollectionView As ICollectionView
            Get
                If EpisodesCollectionViewSource IsNot Nothing Then
                    Return EpisodesCollectionViewSource.View
                End If
                Return Nothing
            End Get
        End Property

        Private _filterAmount As Integer = 1
        Public Property FilterAmount As Integer
            Get
                Return _filterAmount
            End Get
            Set(value As Integer)
                If SetProperty(_filterAmount, value) Then
                    OnFilterChanged()
                End If
            End Set
        End Property

        Private _filterIncrement As String = "Months"
        Public Property FilterIncrement As String
            Get
                Return _filterIncrement
            End Get
            Set(value As String)
                If SetProperty(_filterIncrement, value) Then
                    OnFilterChanged()
                End If
            End Set
        End Property

#End Region

        Private Sub InitializeEpisodesCollecionViewSource()
            Windows.Application.Current.Dispatcher.Invoke(
                Sub()
                    EpisodesCollectionViewSource = New CollectionViewSource() With {.Source = innerEpisodes}
                    EpisodesCollectionViewSource.GroupDescriptions.Add(New PropertyGroupDescription("TvShow.Name"))
                    AddHandler EpisodesCollectionViewSource.Filter, AddressOf EpisodesCollection_Filter
                    OnPropertyChanged("EpisodesCollectionView")
                End Sub)
        End Sub

        Private Sub OnFilterChanged()
            If EpisodesCollectionViewSource IsNot Nothing AndAlso
                           EpisodesCollectionViewSource.View IsNot Nothing Then
                EpisodesCollectionViewSource.View.Refresh()
            End If
        End Sub

        Private Sub EpisodesCollection_Filter(sender As Object, e As FilterEventArgs)
            Dim cutOffDate As Date = Date.Now()
            Select Case FilterIncrement
                Case "Days"
                    cutOffDate = cutOffDate.AddDays(FilterAmount - 1)
                Case "Weeks"
                    cutOffDate = cutOffDate.AddDays((FilterAmount * 7) - 1)
                Case "Months"
                    cutOffDate = cutOffDate.AddMonths(FilterAmount).AddDays(-1)
            End Select
            Dim ep As Episode = TryCast(e.Item, Episode)
            e.Accepted = ep.AirDate.HasValue AndAlso ep.AirDate.Value >= Date.Now().AddDays(-1) AndAlso ep.AirDate.Value <= cutOffDate
        End Sub

        Private _timeIncrements As List(Of String)
        Public ReadOnly Property TimeIncrements As List(Of String)
            Get
                If _timeIncrements Is Nothing Then
                    _timeIncrements = New List(Of String) From {"Days", "Weeks", "Months"}
                End If
                Return _timeIncrements
            End Get
        End Property

    End Class

End Namespace
