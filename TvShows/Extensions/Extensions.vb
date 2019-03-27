Module Extensions

    <Runtime.CompilerServices.Extension>
    Public Function Slice(aString As String, Optional start As Integer? = Nothing, Optional [end] As Integer? = Nothing) As String
        If start.HasValue AndAlso [end].HasValue AndAlso start.Value > [end].Value Then
            Throw New ArgumentException("starting point must be less than or equal to ending point", "start")
        End If
        If start.HasValue AndAlso start < 0 Then
            start = aString.Length + start
        End If

        If [end].HasValue AndAlso [end] < 0 Then
            [end] = aString.Count + [end]
        End If

        If Not start.HasValue Then
            start = 0
        End If

        If Not [end].HasValue Then
            [end] = aString.Length
        End If

        Return String.Concat(aString.Skip(start.Value).Take([end].Value - start.Value))

    End Function

    <Runtime.CompilerServices.Extension>
    Public Function OnlineInfoEquals(t1 As Model.TvShow, t2 As Model.TvShow) As Boolean
        Return t1.HomePage = t2.HomePage AndAlso
               t1.Name = t2.Name AndAlso
               t1.OriginCountry = t2.OriginCountry AndAlso
               t1.Overview = t2.Overview AndAlso
               t1.PosterPath = t2.PosterPath AndAlso
               t1.BackdropPath = t2.BackdropPath
    End Function

End Module
