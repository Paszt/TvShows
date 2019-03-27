Public Class SearchTextBox
    Inherits System.Windows.Controls.Control

    Shared Sub New()
        'This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
        'This style is defined in themes\generic.xaml
        DefaultStyleKeyProperty.OverrideMetadata(GetType(SearchTextBox), New FrameworkPropertyMetadata(GetType(SearchTextBox)))
    End Sub


#Region " Dependency Properties "

    Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String),
                                                                                            GetType(SearchTextBox),
                                                                                            New FrameworkPropertyMetadata(Nothing,
                                                                                                                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                                                                          Nothing, Nothing, True, UpdateSourceTrigger.PropertyChanged))

    Public Property Text() As String
        Get
            Return CType(GetValue(TextProperty), String)
        End Get
        Set(value As String)
            SetValue(TextProperty, value)
        End Set
    End Property

#End Region


End Class
