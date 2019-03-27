Imports System.Runtime.Serialization

Namespace Model

    <DataContract>
    Public Class Season

        <DataMember(Name:="air_date")>
        Public Property AirDate As String

        <DataMember(Name:="episodes")>
        Public Property Episodes As List(Of Episode)

        <DataMember(Name:="name")>
        Public Property Name As String

        <DataMember(Name:="overview")>
        Public Property Overview As String

        <DataMember(Name:="id")>
        Public Property Id As Integer

        <DataMember(Name:="poster_path")>
        Public Property PosterPath As String

        <DataMember(Name:="season_number")>
        Public Property SeasonNumber As Integer

    End Class

End Namespace
