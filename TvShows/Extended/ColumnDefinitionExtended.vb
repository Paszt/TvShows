Namespace Extended

    Public Class ColumnDefinitionExtended
        Inherits ColumnDefinition

        ' Variables
        Public Shared VisibleProperty As DependencyProperty

        ' Properties
        Public Property Visible() As Boolean
            Get
                Return DirectCast(GetValue(VisibleProperty), Boolean)
            End Get
            Set
                SetValue(VisibleProperty, Value)
            End Set
        End Property

        ' Constructors
        Shared Sub New()
            VisibleProperty = DependencyProperty.Register("Visible",
                                                          GetType(Boolean),
                                                          GetType(ColumnDefinitionExtended),
                                                          New PropertyMetadata(True, New PropertyChangedCallback(AddressOf OnVisibleChanged)))

            WidthProperty.OverrideMetadata(GetType(ColumnDefinitionExtended),
                                           New FrameworkPropertyMetadata(New GridLength(1, GridUnitType.Star),
                                                                         Nothing,
                                                                         New CoerceValueCallback(AddressOf CoerceWidth)))

            MinWidthProperty.OverrideMetadata(GetType(ColumnDefinitionExtended),
                                              New FrameworkPropertyMetadata(0.00,
                                                                            Nothing,
                                                                            New CoerceValueCallback(AddressOf CoerceMinWidth)))
        End Sub

        ' Get/Set
        Public Shared Sub SetVisible(obj As DependencyObject, nVisible As Boolean)
            obj.SetValue(VisibleProperty, nVisible)
        End Sub

        Public Shared Function GetVisible(obj As DependencyObject) As Boolean
            Return DirectCast(obj.GetValue(VisibleProperty), Boolean)
        End Function

        Private Shared Sub OnVisibleChanged(obj As DependencyObject, e As DependencyPropertyChangedEventArgs)
            obj.CoerceValue(WidthProperty)
            obj.CoerceValue(MinWidthProperty)
        End Sub

        Private Shared Function CoerceWidth(obj As DependencyObject, nValue As Object) As Object
            Return If((DirectCast(obj, ColumnDefinitionExtended).Visible), nValue, New GridLength(0))
        End Function

        Private Shared Function CoerceMinWidth(obj As DependencyObject, nValue As Object) As Object
            Return If((DirectCast(obj, ColumnDefinitionExtended).Visible), nValue, 0.00)
        End Function

    End Class

End Namespace
