Imports System.Globalization

Namespace Infrastructure

    <ValueConversion(GetType(Object), GetType(String))>
    Public Class PosterPathConverter
        Inherits BaseConverter
        Implements IValueConverter

        'OrElse ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject())
        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim size As String = "w185"
            If parameter IsNot Nothing Then
                size = parameter.ToString()
            End If

            If value Is Nothing OrElse String.IsNullOrWhiteSpace(value.ToString()) Then
                Return NoPosterDrawingImage()
            Else
                If ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
                    Return "http://image.tmdb.org/t/p/" & size & value.ToString()
                Else
                    Return My.Application.TmdbImageBaseUrl & size & value.ToString()
                End If
            End If
        End Function

        Public Function ConvertBack(value As Object,
                                    targetType As Type,
                                    parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

        Private Function NoPosterDrawingImage() As DrawingImage
            Dim dg As New DrawingGroup()
            Dim lightGreyBrush As New SolidColorBrush(CType(ColorConverter.ConvertFromString("#FFAFAFAF"), Color))
            Dim darkGreyBrush As New SolidColorBrush(CType(ColorConverter.ConvertFromString("#FF666666"), Color))
            Using dc As DrawingContext = dg.Open()
                dc.DrawGeometry(lightGreyBrush, Nothing, Geometry.Parse("F1 M 0,366.667L 246.667,366.667L 246.667,3.05176e-005L 0,3.05176e-005L 0,366.667 Z "))
                dc.DrawGeometry(darkGreyBrush, Nothing, Geometry.Parse("F1 M 184.667,100L 171.333,100L 171.333,85.3334L 184.667,85.3334M 184.667,121.333L 171.333,121.333L 171.333,106.667L 184.667,106.667M 184.667,142.667L 171.333,142.667L 171.333,128L 184.667,128M 184.667,164L 171.333,164L 171.333,149.333L 184.667,149.333M 184.667,185.333L 171.333,185.333L 171.333,170.667L 184.667,170.667M 184.667,206.667L 171.333,206.667L 171.333,192L 184.667,192M 162,140L 84.6667,140L 84.6667,85.3334L 162,85.3334M 162,206.667L 84.6667,206.667L 84.6667,152L 162,152M 75.3333,100L 62,100L 62,85.3334L 75.3333,85.3334M 75.3333,121.333L 62,121.333L 62,106.667L 75.3333,106.667M 75.3333,142.667L 62,142.667L 62,128L 75.3333,128M 75.3333,164L 62,164L 62,149.333L 75.3333,149.333M 75.3333,185.333L 62,185.333L 62,170.667L 75.3333,170.667M 75.3333,206.667L 62,206.667L 62,192L 75.3333,192M 51.3333,220L 195.333,220L 195.333,72L 51.3333,72L 51.3333,220 Z "))
                dc.DrawGeometry(darkGreyBrush, Nothing, Geometry.Parse("F1 M 215.306,289.016L 215.921,289.016C 217.991,289.016 220.321,288.628 220.321,285.976C 220.321,283.321 217.991,282.933 215.921,282.933L 215.306,282.933M 229.249,302.473L 221.355,302.473L 215.371,293.092L 215.306,293.092L 215.306,302.473L 208.966,302.473L 208.966,278.082L 218.443,278.082C 223.265,278.082 226.919,280.378 226.919,285.588C 226.919,288.95 225.043,291.862 221.582,292.477M 195.902,283.452L 195.902,287.528L 203.053,287.528L 203.053,292.897L 195.902,292.897L 195.902,297.102L 203.441,297.102L 203.441,302.473L 189.562,302.473L 189.562,278.082L 203.441,278.082L 203.441,283.452M 180.414,302.473L 174.074,302.473L 174.074,283.452L 168.833,283.452L 168.833,278.082L 185.655,278.082L 185.655,283.452L 180.414,283.452M 162.562,284.358C 161.398,283.386 159.91,282.74 158.358,282.74C 157.193,282.74 155.641,283.42 155.641,284.777C 155.641,286.202 157.354,286.752 158.454,287.106L 160.071,287.593C 163.469,288.596 166.089,290.31 166.089,294.257C 166.089,296.682 165.506,299.173 163.565,300.822C 161.658,302.44 159.102,303.12 156.643,303.12C 153.57,303.12 150.561,302.085 148.07,300.337L 150.787,295.226C 152.374,296.617 154.249,297.75 156.417,297.75C 157.905,297.75 159.49,297.005 159.49,295.292C 159.49,293.512 156.999,292.897 155.641,292.509C 151.662,291.377 149.042,290.342 149.042,285.618C 149.042,280.669 152.567,277.434 157.451,277.434C 159.91,277.434 162.919,278.21 165.086,279.44M 124.238,289.857C 124.238,294.062 127.345,297.136 131.129,297.136C 134.914,297.136 138.019,294.062 138.019,289.857C 138.019,286.492 134.914,283.42 131.129,283.42C 127.345,283.42 124.238,286.492 124.238,289.857 Z M 144.618,289.824C 144.618,297.717 138.957,303.281 131.129,303.281C 123.301,303.281 117.639,297.717 117.639,289.824C 117.639,282.449 124.11,277.273 131.129,277.273C 138.149,277.273 144.618,282.449 144.618,289.824 Z M 102.217,289.274L 103.283,289.274C 105.581,289.274 107.845,289.274 107.845,286.298C 107.845,283.225 105.742,283.193 103.283,283.193L 102.217,283.193M 95.8753,278.082L 105.581,278.082C 110.821,278.082 114.445,280.508 114.445,286.104C 114.445,291.862 111.338,294.385 105.807,294.385L 102.217,294.385L 102.217,302.473L 95.8753,302.473M 58.234,289.857C 58.234,294.062 61.3393,297.136 65.1247,297.136C 68.91,297.136 72.0153,294.062 72.0153,289.857C 72.0153,286.492 68.91,283.42 65.1247,283.42C 61.3393,283.42 58.234,286.492 58.234,289.857 Z M 78.614,289.824C 78.614,297.717 72.9527,303.281 65.1247,303.281C 57.2967,303.281 51.6353,297.717 51.6353,289.824C 51.6353,282.449 58.1047,277.273 65.1247,277.273C 72.1447,277.273 78.614,282.449 78.614,289.824 Z M 22.7567,278.082L 29.0967,278.082L 40.71,292.994L 40.7753,292.994L 40.7753,278.082L 47.1153,278.082L 47.1153,302.473L 40.7753,302.473L 29.162,287.528L 29.0967,287.528L 29.0967,302.473L 22.7567,302.473L 22.7567,278.082 Z "))
            End Using
            Return New DrawingImage(dg)
        End Function

    End Class

    <ValueConversion(GetType(Object), GetType(String))>
    Public Class BackdropPathConverter
        Inherits BaseConverter
        Implements IValueConverter

        'OrElse ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject())
        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim size As String = "w300"
            If parameter IsNot Nothing Then
                size = parameter.ToString()
            End If

            If value Is Nothing OrElse String.IsNullOrWhiteSpace(value.ToString()) Then
                Return NoBackdropDrawingImage()
            Else
                If ComponentModel.DesignerProperties.GetIsInDesignMode(New DependencyObject()) Then
                    Return "http://image.tmdb.org/t/p/" & size & value.ToString()
                Else
                    Return My.Application.TmdbImageBaseUrl & size & value.ToString()
                End If
            End If
        End Function

        Public Function ConvertBack(value As Object,
                                    targetType As Type,
                                    parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

        Private Function NoBackdropDrawingImage() As DrawingImage
            Dim dg As New DrawingGroup()
            Dim lightGreyBrush As New SolidColorBrush(CType(ColorConverter.ConvertFromString("#FFAFAFAF"), Color))
            Dim darkGreyBrush As New SolidColorBrush(CType(ColorConverter.ConvertFromString("#FF666666"), Color))
            Using dc As DrawingContext = dg.Open()
                dc.DrawGeometry(lightGreyBrush, Nothing, Geometry.Parse("F1 M 0,192L 341.333,192L 341.333,0L 0,0L 0,192 Z "))
                dc.DrawGeometry(darkGreyBrush, Nothing, Geometry.Parse("F1 M 214.444,42.5333L 204.221,42.5333L 204.221,32.3107L 214.444,32.3107M 214.444,58.5333L 204.221,58.5333L 204.221,48.2227L 214.444,48.2227M 214.444,74.444L 204.221,74.444L 204.221,63.0667L 214.444,63.0667M 214.444,90.356L 204.221,90.356L 204.221,78.9787L 214.444,78.9787M 214.444,106.177L 204.221,106.177L 204.221,94.888L 214.444,94.888M 214.444,121.023L 204.221,121.023L 204.221,110.712L 214.444,110.712M 197.377,72.1334L 140.488,72.1334L 140.488,32.3107L 197.377,32.3107M 197.377,121.023L 140.488,121.023L 140.488,81.2894L 197.377,81.2894M 133.644,42.5333L 123.421,42.5333L 123.421,32.3107L 133.644,32.3107M 133.644,58.5333L 123.421,58.5333L 123.421,48.2227L 133.644,48.2227M 133.644,74.444L 123.421,74.444L 123.421,63.0667L 133.644,63.0667M 133.644,90.356L 123.421,90.356L 123.421,78.9787L 133.644,78.9787M 133.644,106.177L 123.421,106.177L 123.421,94.888L 133.644,94.888M 133.644,121.023L 123.421,121.023L 123.421,110.712L 133.644,110.712M 115.511,131.244L 222.444,131.244L 222.444,22.1773L 115.511,22.1773L 115.511,131.244 Z "))
                dc.DrawGeometry(darkGreyBrush, Nothing, Geometry.Parse("F1 M 253.73,153.677L 253.73,157.346L 260.166,157.346L 260.166,162.181L 253.73,162.181L 253.73,165.968L 260.517,165.968L 260.517,170.802L 248.022,170.802L 248.022,148.842L 260.517,148.842L 260.517,153.677M 242.487,158.541C 242.43,161.745 242.197,164.482 240.245,167.22C 238.09,170.22 234.857,171.53 231.187,171.53C 224.285,171.53 219.479,166.841 219.479,159.938C 219.479,152.804 224.314,148.114 231.391,148.114C 235.906,148.114 239.401,150.153 241.294,154.26L 235.877,156.532C 235.09,154.434 233.43,153.036 231.129,153.036C 227.373,153.036 225.421,156.618 225.421,159.968C 225.421,163.376 227.459,166.841 231.217,166.841C 233.693,166.841 235.585,165.56 235.818,163.026L 231.158,163.026L 231.158,158.541M 204.51,155.629L 204.451,155.629L 202.091,162.618L 206.839,162.618M 200.519,166.988L 199.005,170.802L 192.947,170.802L 201.393,148.842L 207.626,148.842L 215.897,170.802L 209.81,170.802L 208.383,166.988M 165.803,148.842L 171.423,148.842L 175.879,160.58L 180.598,148.842L 186.278,148.842L 189.598,170.802L 183.889,170.802L 182.287,158.162L 182.229,158.162L 176.958,170.802L 174.686,170.802L 169.647,158.162L 169.589,158.162L 167.754,170.802L 162.075,170.802M 156.745,170.802L 151.037,170.802L 151.037,148.842L 156.745,148.842M 114.223,159.444C 114.223,163.23 117.019,165.997 120.427,165.997C 123.835,165.997 126.63,163.23 126.63,159.444C 126.63,156.414 123.835,153.648 120.427,153.648C 117.019,153.648 114.223,156.414 114.223,159.444 Z M 132.573,159.414C 132.573,166.521 127.475,171.53 120.427,171.53C 113.379,171.53 108.282,166.521 108.282,159.414C 108.282,152.774 114.107,148.114 120.427,148.114C 126.747,148.114 132.573,152.774 132.573,159.414 Z M 80.8167,148.842L 86.526,148.842L 96.982,162.269L 97.0393,162.269L 97.0393,148.842L 102.749,148.842L 102.749,170.802L 97.0393,170.802L 86.5847,157.346L 86.526,157.346L 86.526,170.802L 80.8167,170.802L 80.8167,148.842 Z "))
            End Using
            Return New DrawingImage(dg)
        End Function

    End Class

End Namespace
