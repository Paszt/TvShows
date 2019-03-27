Namespace Tmdb

    Public Class Urls

        Const baseUrl = "https://api.themoviedb.org/3/"

        Public Shared Function TvShow(id As Integer, Language As Iso639_1.Code) As String
            Dim urlFormat = baseUrl & "tv/{0}?langauge={1}&append_to_response=external_ids&api_key={2}"
            Return String.Format(urlFormat, id, Language.ToString(), Api.Key)
        End Function

        Public Shared Function TvSeason(TmdbShowId As Integer, SeasonNumber As Integer, Language As Iso639_1.Code) As String
            Dim urlFormat = baseUrl & "tv/{0}/season/{1}?language={2}&api_key={3}"
            Return String.Format(urlFormat, TmdbShowId, SeasonNumber, Language.ToString(), Api.Key)
        End Function

        Public Overloads Shared Function TvShowSearch(query As String, language As Iso639_1.Code) As String
            Dim urlFormat = baseUrl + "search/tv?query={0}&language={1}&api_key={2}"
            query = Net.WebUtility.UrlEncode(query)
            Return String.Format(urlFormat, query, language.ToString(), Api.Key)
        End Function

        Public Overloads Shared Function TvShowSearch(query As String, language As Iso639_1.Code, year As Integer) As String
            Return TvShowSearch(query, language) & "&year=" & year
        End Function

        Public Shared Function Configuration() As String
            Return baseUrl & "configuration?api_key=" & Api.Key
        End Function

    End Class

End Namespace
