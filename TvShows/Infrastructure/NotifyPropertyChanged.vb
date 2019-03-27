Imports System.ComponentModel

Namespace Infrastructure

    <Runtime.Serialization.DataContract>
    Public Class NotifyPropertyChanged
        Implements INotifyPropertyChanged

#Region " INotifyPropertyChanged Members "

        Public Event PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Function SetProperty(Of T)(ByRef storage As T, value As T,
                                         <Runtime.CompilerServices.CallerMemberName> Optional propertyName As String = Nothing) As Boolean
            If Equals(storage, value) Then
                Return False
            End If
            storage = value
            OnPropertyChanged(propertyName)
            Return True
        End Function

        Protected Sub OnPropertyChanged(<Runtime.CompilerServices.CallerMemberName> Optional propertyName As String = Nothing)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If propertyChanged IsNot Nothing Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub

#End Region

    End Class

End Namespace
