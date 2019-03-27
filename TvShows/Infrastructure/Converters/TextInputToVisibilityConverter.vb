Imports System.Globalization

Namespace Infrastructure

    Public Class TextInputToVisibilityConverter
        Inherits BaseConverter
        Implements IMultiValueConverter

        Public Function Convert(values() As Object, targetType As Type,
                                parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            ' Always test MultiValueConverter inputs for non-null
            ' (to avoid crash bugs for views in the designer)
            If TypeOf values(0) Is Boolean AndAlso TypeOf values(1) Is Boolean Then
                Dim hasText As Boolean = Not CBool(values(0))
                Dim hasFocus As Boolean = CBool(values(1))

                If hasFocus OrElse hasText Then
                    Return Visibility.Collapsed
                End If
            End If

            Return Visibility.Visible
        End Function

        Public Function ConvertBack(value As Object, targetTypes() As Type,
                                    parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
