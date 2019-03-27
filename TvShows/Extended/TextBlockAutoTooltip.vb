Namespace Extended

    Public NotInheritable Class TextBlockAutoTooltip

        Shared Sub New()
            ' Register for the SizeChanged event on all TextBlocks, even if the event was handled.
            EventManager.RegisterClassHandler(GetType(TextBlock),
                                              FrameworkElement.SizeChangedEvent,
                                              New SizeChangedEventHandler(AddressOf OnTextBlockSizeChanged), True)
        End Sub

        Private Shared ReadOnly IsTextTrimmedKey As DependencyPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("IsTextTrimmed",
                                                        GetType(Boolean),
                                                        GetType(TextBlockAutoTooltip),
                                                        New PropertyMetadata(False))

        Public Shared ReadOnly IsTextTrimmedProperty As DependencyProperty = IsTextTrimmedKey.DependencyProperty

        <AttachedPropertyBrowsableForType(GetType(TextBlock))>
        Public Shared Function GetIsTextTrimmed(target As TextBlock) As Boolean
            Return DirectCast(target.GetValue(IsTextTrimmedProperty), Boolean)
        End Function

        Public Shared ReadOnly AutomaticToolTipEnabledProperty As DependencyProperty =
            DependencyProperty.RegisterAttached("AutomaticToolTipEnabled",
                                                GetType(Boolean),
                                                GetType(TextBlockAutoTooltip),
                                                New FrameworkPropertyMetadata(True,
                                                                              FrameworkPropertyMetadataOptions.Inherits))

        <AttachedPropertyBrowsableForType(GetType(DependencyObject))>
        Public Shared Function GetAutomaticToolTipEnabled(element As DependencyObject) As Boolean
            If element Is Nothing Then
                Throw New ArgumentNullException("element")
            End If
            Return CBool(element.GetValue(AutomaticToolTipEnabledProperty))
        End Function

        Public Shared Sub SetAutomaticToolTipEnabled(element As DependencyObject, value As Boolean)
            If element Is Nothing Then
                Throw New ArgumentNullException("element")
            End If
            element.SetValue(AutomaticToolTipEnabledProperty, value)
        End Sub

        Private Shared Sub OnTextBlockSizeChanged(sender As Object, e As SizeChangedEventArgs)
            TriggerTextRecalculation(sender)
        End Sub

        Private Shared Sub TriggerTextRecalculation(sender As Object)
            Dim textBlock = TryCast(sender, TextBlock)
            If textBlock Is Nothing Then
                Return
            End If

            If TextTrimming.None = textBlock.TextTrimming Then
                textBlock.SetValue(IsTextTrimmedKey, False)
            Else
                'If this function is called before databinding has finished the tooltip will never show.
                'This invoke defers the calculation of the text trimming till after all current pending databinding
                'has completed.
                Dim isTextTrimmed = textBlock.Dispatcher.Invoke(Function() CalculateIsTextTrimmed(textBlock),
                                                                Threading.DispatcherPriority.DataBind)
                textBlock.SetValue(IsTextTrimmedKey, isTextTrimmed)
            End If
        End Sub

        Private Shared Function CalculateIsTextTrimmed(textBlock As TextBlock) As Boolean
            If Not textBlock.IsArrangeValid Then
                Return GetIsTextTrimmed(textBlock)
            End If

            Dim typeface As New Typeface(textBlock.FontFamily,
                                         textBlock.FontStyle,
                                         textBlock.FontWeight,
                                         textBlock.FontStretch)

            ' FormattedText is used to measure the whole width of the text held up by TextBlock container
            Dim formattedText As New FormattedText(textBlock.Text,
                                                   Threading.Thread.CurrentThread.CurrentCulture,
                                                   textBlock.FlowDirection,
                                                   typeface,
                                                   textBlock.FontSize,
                                                   textBlock.Foreground) With {
                .MaxTextWidth = textBlock.ActualWidth}

            ' When the maximum text width of the FormattedText instance is set to the actual
            ' width of the textBlock, if the textBlock is being trimmed to fit then the formatted
            ' text will report a larger height than the textBlock. Should work whether the
            ' textBlock is single or multi-line.
            ' The width check detects if any single line is too long to fit within the text area, 
            ' this can only happen if there is a long span of text with no spaces.
            Return (formattedText.Height > textBlock.ActualHeight OrElse formattedText.MinWidth > formattedText.MaxTextWidth)
        End Function

    End Class

End Namespace
