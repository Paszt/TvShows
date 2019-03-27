Namespace Model

    Public Class EpisodeSorter
        Implements IComparer

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim xEp As Episode = TryCast(x, Episode)
            Dim yEp As Episode = TryCast(y, Episode)

            If xEp.SeasonNumber = yEp.SeasonNumber Then
                Return yEp.EpisodeNumber.CompareTo(xEp.EpisodeNumber)
            End If

            If xEp.SeasonNumber = 0 Then
                Return 1
            ElseIf yEp.SeasonNumber = 0 Then
                Return -1
            Else
                Return yEp.SeasonNumber.CompareTo(xEp.SeasonNumber)
            End If
        End Function

    End Class

End Namespace
