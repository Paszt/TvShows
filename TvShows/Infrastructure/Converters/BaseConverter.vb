Imports System.Windows.Markup

Namespace Infrastructure

    Public MustInherit Class BaseConverter
        Inherits MarkupExtension

        Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
            Return Me
        End Function

    End Class

End Namespace
