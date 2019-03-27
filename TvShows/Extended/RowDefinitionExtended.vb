Namespace Extended

    Public Class RowDefinitionExtended
        Inherits RowDefinition

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
                                                          GetType(RowDefinitionExtended),
                                                          New PropertyMetadata(True, New PropertyChangedCallback(AddressOf OnVisibleChanged)))

            HeightProperty.OverrideMetadata(GetType(RowDefinitionExtended),
                                           New FrameworkPropertyMetadata(New GridLength(1, GridUnitType.Star),
                                                                         Nothing,
                                                                         New CoerceValueCallback(AddressOf CoerceHeight)))

            MinHeightProperty.OverrideMetadata(GetType(RowDefinitionExtended),
                                              New FrameworkPropertyMetadata(0.00,
                                                                            Nothing,
                                                                            New CoerceValueCallback(AddressOf CoerceMinHeight)))
        End Sub

        ' Get/Set
        Public Shared Sub SetVisible(obj As DependencyObject, nVisible As Boolean)
            obj.SetValue(VisibleProperty, nVisible)
        End Sub

        Public Shared Function GetVisible(obj As DependencyObject) As Boolean
            Return DirectCast(obj.GetValue(VisibleProperty), Boolean)
        End Function

        Private Shared Sub OnVisibleChanged(obj As DependencyObject, e As DependencyPropertyChangedEventArgs)
            obj.CoerceValue(HeightProperty)
            obj.CoerceValue(MinHeightProperty)
        End Sub

        Private Shared Function CoerceHeight(obj As DependencyObject, nValue As Object) As Object
            Return If((DirectCast(obj, RowDefinitionExtended).Visible), nValue, New GridLength(0))
        End Function

        Private Shared Function CoerceMinHeight(obj As DependencyObject, nValue As Object) As Object
            Return If((DirectCast(obj, RowDefinitionExtended).Visible), nValue, 0.00)
        End Function

    End Class

End Namespace
