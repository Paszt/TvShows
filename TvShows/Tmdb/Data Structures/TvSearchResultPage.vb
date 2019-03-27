Imports System.Runtime.Serialization

Namespace Tmdb

    <DataContract>
    Public Class TvSearchResultPage

        <DataMember(Name:="page")>
        Public Property Page As Integer

        <DataMember(Name:="results")>
        Public Property Results As List(Of Model.TvShow)

        <DataMember(Name:="total_pages")>
        Public Property TotalPages As Integer

        <DataMember(Name:="total_results")>
        Public Property TotalResults As Integer

    End Class

End Namespace
