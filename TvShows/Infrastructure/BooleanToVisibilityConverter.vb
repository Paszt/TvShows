Imports System.Globalization

Namespace Infrastructure

    <ValueConversion(GetType(Boolean), GetType(Visibility))>
    Public NotInheritable Class BooleanToVisibilityConverter
        Implements IValueConverter

        Public Property TrueValue() As Visibility
        Public Property FalseValue() As Visibility

        Public Sub New()
            ' set defaults
            TrueValue = Visibility.Visible
            FalseValue = Visibility.Collapsed
        End Sub

        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            If Not (TypeOf value Is Boolean) Then
                Return Nothing
            End If
            If CBool(value) Then
                Return TrueValue
            Else
                Return FalseValue
            End If
        End Function

        Public Function ConvertBack(value As Object,
                                    targetType As Type,
                                    parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If Equals(value, TrueValue) Then
                Return True
            End If
            If Equals(value, FalseValue) Then
                Return False
            End If
            Return Nothing
        End Function
    End Class

End Namespace
