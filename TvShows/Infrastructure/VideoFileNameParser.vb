Imports System.Text.RegularExpressions

Namespace Infrastructure

    Public Class VideoFileNameParser

        Public Property IsFileName As Boolean

        Public Sub New(Optional isFileName As Boolean = True)
            Me.IsFileName = isFileName
        End Sub

        ''' <summary>
        ''' Cleans up series name by removing any . and _
        ''' characters, along with any trailing hyphens.
        ''' Is basically equivalent to replacing all _ And . with a
        ''' space, but handles decimal numbers in string, for example:
        '''     cleanRegexedSeriesName("an.example.1.0.test")  => an example 1.0 test
        '''     cleanRegexedSeriesName("an_example_1.0_test")  => an example 1.0 test
        ''' </summary>
        Public Function CleanSeriesName(seriesName As String) As String
            seriesName = Regex.Replace(seriesName, "(\D)\.(?!\s)(\D)", "$1 $2")
            seriesName = Regex.Replace(seriesName, "(\d)\.(\d{4})", "$1 $2") 'if it ends in a year then don't keep the dot
            seriesName = Regex.Replace(seriesName, "(\D)\.(?!\s)", "$1 ")
            seriesName = Regex.Replace(seriesName, "\.(?!\s)(\D)", " $1")
            seriesName = seriesName.Replace("_", " ")
            seriesName = Regex.Replace(seriesName, "-$", "")

            Return seriesName.Trim()
        End Function

        Private Function ParseString(name As String) As VideoFileParseResult
            If String.IsNullOrWhiteSpace(name) Then
                Return Nothing
            End If

            For Each regExPattern In EpisodeRegex.Patterns
                Dim re = New Regex(regExPattern.Value, RegexOptions.IgnoreCase)
                Dim match = re.Match(name)
                If Not match.Success Then Continue For

                Console.WriteLine("Match for """ & name & """ found using " & regExPattern.Key)

                Dim result = New VideoFileParseResult(name)
                Dim namedGroups As String() = re.GetGroupNames()

                If namedGroups.Contains("series_name") Then
                    result.SeriesName = match.Groups("series_name").Value
                    If Not String.IsNullOrWhiteSpace(result.SeriesName) Then
                        result.SeriesName = CleanSeriesName(result.SeriesName)
                    End If
                End If
                If namedGroups.Contains("season_num") Then
                    Dim tmpSeason As Integer = CInt(match.Groups("season_num").Value)
                    If regExPattern.Key = "bare" AndAlso (tmpSeason = 19 Or tmpSeason = 20) Then Continue For
                    result.SeasonNumber = tmpSeason
                End If
                If namedGroups.Contains("ep_num") Then
                    result.EpisodeNumbers = New List(Of Integer)
                    Try
                        Dim epNum = ConvertNumber(match.Groups("ep_num").Value)
                        If namedGroups.Contains("extra_ep_num") AndAlso Not String.IsNullOrEmpty(match.Groups("extra_ep_num").Value) Then
                            For i = epNum To ConvertNumber(match.Groups("extra_ep_num").Value)
                                result.EpisodeNumbers.Add(i)
                            Next
                        Else
                            result.EpisodeNumbers.Add(epNum)
                        End If
                    Catch ex As ArgumentException
                        Throw New InvalidNameException(ex)
                    End Try
                End If
                If namedGroups.Contains("air_year") AndAlso namedGroups.Contains("air_month") AndAlso namedGroups.Contains("air_day") Then
                    Dim year = CInt(match.Groups("air_year").Value)
                    Dim month = CInt(match.Groups("air_month").Value)
                    Dim day = CInt(match.Groups("air_day").Value)
                    Dim tmpMonth As Integer
                    'make an attempt to detect YYYY-DD-MM formats (swap month with day)
                    If month > 12 Then
                        tmpMonth = month
                        month = day
                        day = tmpMonth
                    End If

                    Dim dateString = year & "-" & month & "-" & day
                    If Date.TryParse(dateString, New Date) Then
                        result.AirDate = dateString
                    Else
                        Throw New InvalidNameException()
                    End If
                End If

                result.IsProper = False

                If namedGroups.Contains("extra_info") Then
                    Dim tmpExtraInfo = match.Groups("extra_info").Value
                    'check if it's a proper
                    If Not String.IsNullOrWhiteSpace(tmpExtraInfo) Then
                        result.IsProper = Regex.IsMatch(match.Groups("extra_info").Value, "(^|[\. _-])(proper|repack)([\. _-]|$)", RegexOptions.IgnoreCase)
                    End If

                    'Show.S04.Special or Show.S05.Part.2.Extras is almost certainly not every episode in the season
                    If Not String.IsNullOrWhiteSpace(tmpExtraInfo) AndAlso
                            regExPattern.Key = "season_only" AndAlso
                            Regex.IsMatch(tmpExtraInfo, "([. _-]|^)(special|extra)s?\w*([. _-]|$)", RegexOptions.IgnoreCase) Then
                        Continue For
                    End If
                    result.ExtraInfo = tmpExtraInfo
                End If

                If namedGroups.Contains("release_group") Then
                    result.ReleaseGroup = match.Groups("release_group").Value
                End If
                Return result
            Next

            Return Nothing

        End Function

        Private Function CombineResults(first As VideoFileParseResult, second As VideoFileParseResult, attr As String) As Object
            'if the first doesn't exist then return the second or nothing
            If first Is Nothing Then
                If second Is Nothing Then
                    Return Nothing
                Else
                    Return CallByName(second, attr, CallType.Get)
                End If
            End If

            'if the second doesn't exist then return the first
            If second Is Nothing Then
                Return first(attr)
            End If

            Dim a = first(attr)
            Dim b = second(attr)

            'if a is good, use it
            If a IsNot Nothing Or (TypeOf a Is IList AndAlso CType(a, IList).Count > 0) Then
                Return a
            Else
                'if not use b
                Return b
            End If
        End Function

        Public Shared Function ConvertNumber(numberString As String) As Integer
            Dim integerValue As Integer
            If Integer.TryParse(numberString, integerValue) Then
                Return integerValue
            End If

            Dim romanNumeralMap As New List(Of RomanNumeral) From {
                New RomanNumeral() With {.Numeral = "M", .Value = 1000, .MaxCount = 3},
                New RomanNumeral() With {.Numeral = "CM", .Value = 900, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "D", .Value = 500, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "CD", .Value = 400, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "C", .Value = 100, .MaxCount = 3},
                New RomanNumeral() With {.Numeral = "XC", .Value = 90, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "L", .Value = 50, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "XL", .Value = 40, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "X", .Value = 10, .MaxCount = 3},
                New RomanNumeral() With {.Numeral = "IX", .Value = 9, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "V", .Value = 5, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "IV", .Value = 4, .MaxCount = 1},
                New RomanNumeral() With {.Numeral = "I", .Value = 1, .MaxCount = 3}
            }

            Dim romanNumeral = numberString.ToUpper()
            integerValue = 0
            Dim index As Integer = 0
            For Each mapEntry In romanNumeralMap
                Dim count As Integer = 0
                While romanNumeral.Slice(index, index + mapEntry.Numeral.Length) = mapEntry.Numeral
                    count += 1
                    If count > mapEntry.MaxCount Then
                        Throw New ArgumentException("Not a roman numeral", "numberString")
                    End If
                    integerValue += mapEntry.Value
                    index += mapEntry.Numeral.Length
                    If index >= romanNumeral.Length Then
                        Exit For
                    End If
                End While
            Next
            If index < romanNumeral.Length Then
                Throw New ArgumentException("Not a roman numeral", "numberString")
            End If
            Return integerValue
        End Function

        Private Class RomanNumeral

            Public Property Numeral As String
            Public Property Value As Integer
            Public Property MaxCount As Integer

        End Class

        Public Function Parse(name As String) As VideoFileParseResult
            'TODO: implement the cache

            'break it into parts if there are any (dirname, file name, extension)
            Dim directoryPath = IO.Path.GetDirectoryName(name)
            Dim directoryName = String.Empty
            If Not String.IsNullOrWhiteSpace(directoryPath) Then
                directoryName = New IO.DirectoryInfo(IO.Path.GetDirectoryName(name)).Name
            End If
            Dim fileName = IO.Path.GetFileNameWithoutExtension(name)

            'set up a result to use
            Dim finalResult As New VideoFileParseResult(name)

            'parsing the file name
            Dim fileNameResult = ParseString(fileName)

            'parse the dirname for extra info if needed
            Dim dirNameResult = ParseString(directoryName)

            'build the ParseResult object
            finalResult.AirDate = CType(CombineResults(fileNameResult, dirNameResult, "AirDate"), String)

            If String.IsNullOrWhiteSpace(finalResult.AirDate) Then
                finalResult.SeasonNumber = CType(CombineResults(fileNameResult, dirNameResult, "SeasonNumber"), Integer)
                finalResult.EpisodeNumbers = CType(CombineResults(fileNameResult, dirNameResult, "EpisodeNumbers"), List(Of Integer))
            End If

            finalResult.IsProper = CType(CombineResults(fileNameResult, dirNameResult, "IsProper"), Boolean)

            'prefer the dirname release group/show name over the filename
            finalResult.SeriesName = CType(CombineResults(dirNameResult, fileNameResult, "SeriesName"), String)
            finalResult.ExtraInfo = CType(CombineResults(dirNameResult, fileNameResult, "ExtraInfo"), String)
            finalResult.ReleaseGroup = CType(CombineResults(dirNameResult, fileNameResult, "ReleaseGroup"), String)

            'if there's no useful info in it then raise an exception
            If finalResult.SeasonNumber Is Nothing AndAlso
                (finalResult.EpisodeNumbers Is Nothing OrElse finalResult.EpisodeNumbers.Count = 0) AndAlso
                String.IsNullOrWhiteSpace(finalResult.AirDate) AndAlso
                String.IsNullOrWhiteSpace(finalResult.SeriesName) Then
                Throw New InvalidNameException("Unable to parse " & name)
            End If

            Return finalResult
        End Function
    End Class

    Public Class VideoFileParseResult

        Public Sub New(original_name As String,
                       Optional series_name As String = Nothing,
                       Optional season_number As Integer? = Nothing,
                       Optional episode_numbers As List(Of Integer) = Nothing,
                       Optional extra_info As String = Nothing,
                       Optional release_group As String = Nothing,
                       Optional air_date As String = Nothing)
            OriginalName = original_name
            SeriesName = series_name
            SeasonNumber = season_number
            EpisodeNumbers = episode_numbers
            ExtraInfo = extra_info
            ReleaseGroup = release_group
            AirDate = air_date
        End Sub

        Public Property OriginalName As String
        Public Property SeriesName As String
        Public Property SeasonNumber As Integer?
        Public Property EpisodeNumbers As List(Of Integer)
        Public Property ExtraInfo As String
        Public Property ReleaseGroup As String
        Public Property AirDate As String
        Public Property IsProper As Boolean

        Public Shared Operator =(ByVal r1 As VideoFileParseResult, ByVal r2 As VideoFileParseResult) As Boolean
            If r1 Is Nothing OrElse r2 Is Nothing Then
                Return False
            End If

            Return r1.SeriesName = r2.SeriesName AndAlso
                   r1.SeasonNumber.Equals(r2.SeasonNumber) AndAlso
                   r1.EpisodeNumbers.SequenceEqual(r2.EpisodeNumbers) AndAlso
                   r1.ExtraInfo = r2.ExtraInfo AndAlso
                   r1.ReleaseGroup = r2.ReleaseGroup AndAlso
                   r1.AirDate = r2.AirDate
        End Operator

        Public Shared Operator <>(ByVal r1 As VideoFileParseResult, ByVal r2 As VideoFileParseResult) As Boolean
            If r1.Equals(r2) Then
                Return False
            End If

            Return r1.SeriesName <> r2.SeriesName AndAlso
                   Not r1.SeasonNumber.Equals(r2.SeasonNumber) AndAlso
                   Not r1.EpisodeNumbers.SequenceEqual(r2.EpisodeNumbers) AndAlso
                   r1.ExtraInfo <> r2.ExtraInfo AndAlso
                   r1.ReleaseGroup <> r2.ReleaseGroup AndAlso
                   r1.AirDate <> r2.AirDate
        End Operator

        Public Overrides Function ToString() As String
            Dim returnValue As String = String.Empty
            If Not String.IsNullOrWhiteSpace(SeriesName) Then
                returnValue = SeriesName & " - "
            End If
            If SeasonNumber.HasValue Then
                returnValue &= "S" & SeasonNumber.Value.ToString("00")
            End If
            If EpisodeNumbers IsNot Nothing AndAlso EpisodeNumbers.Count > 0 Then
                For Each episodeNumber In EpisodeNumbers
                    returnValue &= "E" & episodeNumber.ToString("00")
                Next
            End If
            If IsAirByDate() Then
                returnValue &= AirDate
            End If
            If Not String.IsNullOrWhiteSpace(ExtraInfo) Then
                returnValue &= " - " & ExtraInfo
            End If
            If Not String.IsNullOrWhiteSpace(ReleaseGroup) Then
                returnValue &= "(" & ReleaseGroup & ")"
            End If
            returnValue &= " [ABD: " & IsAirByDate.ToString() & "]"

            Return returnValue
        End Function

        Public Function IsAirByDate() As Boolean
            Return Not SeasonNumber.HasValue AndAlso (EpisodeNumbers Is Nothing OrElse EpisodeNumbers.Count = 0) AndAlso Not String.IsNullOrWhiteSpace(AirDate)
        End Function

        Default Public Property Item(propertyName As String) As Object
            Get
                Dim returnValue = CallByName(Me, propertyName, CallType.Get)
                If TypeOf returnValue Is String AndAlso String.IsNullOrWhiteSpace(CType(returnValue, String)) Then
                    returnValue = Nothing
                End If
                Return returnValue
            End Get
            Set(value As Object)
                CallByName(Me, propertyName, CallType.Set, value)
            End Set
        End Property
    End Class

    Public Class VideoFileParseCache

        Private PreviousParsedList As Dictionary(Of String, VideoFileParseResult)

    End Class

    Public Class InvalidNameException
        Inherits Exception

        Public Sub New()
            MyBase.New("The given name is not valid")
        End Sub

        Public Sub New(ex As Exception)
            MyBase.New("The given name is not valid", ex)
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

    End Class

End Namespace
