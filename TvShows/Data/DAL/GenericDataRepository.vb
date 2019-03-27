
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq.Expressions

Namespace DAL

    Public Class GenericDataRepository(Of T As {Class, Model.IEntity})
        Implements IGenericDataRepository(Of T)

        Public Overridable Function GetAll(ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As IList(Of T) Implements IGenericDataRepository(Of T).GetAll
            Dim list As List(Of T)
            Using context = New Model.TvShowsModelContainer()
                Dim dbQuery As IQueryable(Of T) = context.Set(Of T)()

                'Apply eager loading
                For Each navigationProperty As Expression(Of Func(Of T, Object)) In navigationProperties
                    dbQuery = dbQuery.Include(navigationProperty)
                Next

                'Don't track any changes for the selected item
                list = dbQuery.AsNoTracking().ToList()
            End Using
            Return list
        End Function

        Public Overridable Function GetList(where As Func(Of T, Boolean), ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As IList(Of T) Implements IGenericDataRepository(Of T).GetList
            Dim list As List(Of T)
            Using context = New Model.TvShowsModelContainer()
                Dim dbQuery As IQueryable(Of T) = context.Set(Of T)()

                'Apply eager loading
                For Each navigationProperty As Expression(Of Func(Of T, Object)) In navigationProperties
                    dbQuery = dbQuery.Include(navigationProperty)
                Next

                'Don't track any changes for the selected item
                list = dbQuery.AsNoTracking().Where(where).ToList()
            End Using
            Return list
        End Function

        Public Overridable Function GetSingle(where As Func(Of T, Boolean), ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As T Implements IGenericDataRepository(Of T).GetSingle
            Dim item As T = Nothing
            Using context = New Model.TvShowsModelContainer()
                Dim dbQuery As IQueryable(Of T) = context.Set(Of T)()

                'Apply eager loading
                For Each navigationProperty As Expression(Of Func(Of T, Object)) In navigationProperties
                    dbQuery = dbQuery.Include(navigationProperty)
                Next

                'Don't track any changes for the selected item
                'Apply where clause
                item = dbQuery.AsNoTracking().FirstOrDefault(where)
            End Using
            Return item
        End Function

        Public Function Add(item As T) As Infrastructure.ActionResult Implements IGenericDataRepository(Of T).Add
            If GetSingle(Function(t) t.Id.Equals(item.Id)) IsNot Nothing Then
                Return New Infrastructure.ActionResult() With {
                    .Succeeded = False,
                    .Message = "While adding " & item.Name & ": An object with id " & item.id & " already exists."
                }
            End If
            Return Update(item)
        End Function

        Public Function Remove(item As T) As Infrastructure.ActionResult Implements IGenericDataRepository(Of T).Remove
            Return Update(item)
        End Function

        Public Function Update(item As T) As Infrastructure.ActionResult Implements IGenericDataRepository(Of T).Update
            Dim returnValue As New Infrastructure.ActionResult()
            Try
                Using context = New Model.TvShowsModelContainer()
                    Dim dbSet As DbSet(Of T) = context.Set(Of T)()
                    dbSet.Add(item)
                    For Each entry As DbEntityEntry(Of Model.IEntity) In context.ChangeTracker.Entries(Of Model.IEntity)()
                        Dim entity As Model.IEntity = entry.Entity
                        entry.State = GetEntityState(entity.EntityState)
                    Next
                    context.SaveChanges()
                End Using
            Catch ex As Exception
                returnValue.Succeeded = False
                returnValue.Message = item.Name & " (" & item.Id & ") : " & ex.Message
                'MessageWindow.ShowDialog(ex.Message, "Error adding show")
            End Try
            Return returnValue
        End Function

        Protected Shared Function GetEntityState(entityState As Model.EntityState) As System.Data.Entity.EntityState
            Select Case entityState
                Case Model.EntityState.Unchanged
                    Return System.Data.Entity.EntityState.Unchanged
                Case Model.EntityState.Added
                    Return System.Data.Entity.EntityState.Added
                Case Model.EntityState.Modified
                    Return System.Data.Entity.EntityState.Modified
                Case Model.EntityState.Deleted
                    Return System.Data.Entity.EntityState.Deleted
                Case Else
                    Return System.Data.Entity.EntityState.Detached
            End Select
        End Function

    End Class

End Namespace