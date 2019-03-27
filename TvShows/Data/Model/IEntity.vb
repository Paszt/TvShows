Namespace Model

    Public Interface IEntity
        Property EntityState() As EntityState
        Property Id As Integer?
        Property Name As String
    End Interface

    Public Enum EntityState
        Unchanged
        Added
        Modified
        Deleted
    End Enum

End Namespace
