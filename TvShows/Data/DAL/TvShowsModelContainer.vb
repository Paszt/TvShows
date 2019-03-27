Imports System.Data.Entity

Namespace Model

    Partial Public Class TvShowsModelContainer
        Inherits DbContext

        Public Sub New()
            MyBase.New("name=TvShowsModelContainer")
            Configuration.ProxyCreationEnabled = False
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
            'Throw New UnintentionalCodeFirstException()
            modelBuilder.Entity(Of TvShow).Property(Function(t) t.OriginCountry).IsFixedLength()
            modelBuilder.Entity(Of TvShow).Property(Function(t) t.FirstAirDate).IsFixedLength()

            modelBuilder.Entity(Of Episode).Property(Function(e) e.AirDateString).IsFixedLength()
        End Sub

        Public Overridable Property Episodes() As DbSet(Of Episode)
        Public Overridable Property TvShows() As DbSet(Of TvShow)

    End Class

End Namespace
