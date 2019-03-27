Imports System.Runtime.Serialization

Namespace Tmdb

    <DataContract>
    Public Class Configuration

        <DataMember(Name:="images")>
        Public Property Image As Images

        <DataMember(Name:="change_keys")>
        Public Property ChangeKeys As String()

        <DataContract>
        Public Class Images

            <DataMember(Name:="base_url")>
            Public Property BaseUrl As String

            <DataMember(Name:="secure_base_url")>
            Public Property SecureBaseUrl As String

            <DataMember(Name:="backdrop_sizes")>
            Public Property BackdropSizes As String()

            <DataMember(Name:="logo_sizes")>
            Public Property LogoSizes As String()

            <DataMember(Name:="poster_sizes")>
            Public Property PosterSizes As String()

            <DataMember(Name:="profile_sizes")>
            Public Property ProfileSizes As String()

            <DataMember(Name:="still_sizes")>
            Public Property StillSizes As String()
        End Class

    End Class

End Namespace
