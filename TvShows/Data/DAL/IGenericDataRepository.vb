Imports System.Linq.Expressions

Namespace DAL

    Public Interface IGenericDataRepository(Of T As {Class, Model.IEntity})
        Function GetAll(ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As IList(Of T)
        Function GetList(where As Func(Of T, Boolean), ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As IList(Of T)
        Function GetSingle(where As Func(Of T, Boolean), ParamArray navigationProperties As Expression(Of Func(Of T, Object))()) As T
        Function Add(item As T) As Infrastructure.ActionResult
        Function Update(item As T) As Infrastructure.ActionResult
        Function Remove(item As T) As Infrastructure.ActionResult
    End Interface

End Namespace
