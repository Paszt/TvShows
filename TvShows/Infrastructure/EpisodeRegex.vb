Namespace Infrastructure

    Public Class EpisodeRegex

        Public Shared Function Patterns() As Dictionary(Of String, String)
            Patterns = New Dictionary(Of String, String)

            'standard_repeat   
            '  Show.Name.S01E02.S01E03.Source.Quality.Etc-Group  
            '  Show Name - S01E02 - S01E03 - S01E04 - Ep Name
            Patterns.Add("standard_repeat",
                "^(?<series_name>.+?)[. _-]+" &                   ' Show_Name And separator
                "s(?<season_num>\d+)[. _-]*" &                    ' S01 And optional separator
                "e(?<ep_num>\d+)" &                               ' E02 And separator
                "([. _-]+s\k<season_num>[. _-]*" &                ' S01 And optional separator
                "e(?<extra_ep_num>\d+))+" &                       ' E03/etc And separator
                "[. _-]*((?<extra_info>.+?)" &                    ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                          ' Make sure this Is really the release group
                "-(?<release_group>[^- ]+))?)?$"                  ' Group
            )

            'fov_repeat 
            '  Show.Name.1x02.1x03.Source.Quality.Etc-Group   
            '  Show Name - 1x02 - 1x03 - 1x04 - Ep Name
            Patterns.Add("fov_repeat",
                "^(?<series_name>.+?)[. _-]+" &                   ' Show_Name and separator
                "(?<season_num>\d+)x" &                           ' 1x
                "(?<ep_num>\d+)" &                                ' 02 and separator
                "([. _-]+\k<season_num>x" &                       ' 1x
                "(?<extra_ep_num>\d+))+" &                        ' 03/etc and separator
                "[. _-]*((?<extra_info>.+?)" &                    ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                          ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                  ' Group
            )

            'standard
            '  Show.Name.S01E02.Source.Quality.Etc-Group  
            '  Show Name - S01E02 - My Ep Name  
            '  Show.Name.S01.E03.My.Ep.Name  
            '  Show.Name.S01E02E03.Source.Quality.Etc-Group  
            '  Show Name - S01E02-03 - My Ep Name  
            '  Show.Name.S01.E02.E03
            Patterns.Add("standard",
                "^((?<series_name>.+?)[. _-]+)?" &                 ' Show_Name And separator
                "s(?<season_num>\d+)[. _-]*" &                     ' S01 And optional separator
                "e(?<ep_num>\d+)" &                                ' E02 And separator
                "(([. _-]*e|-)" &                                  ' linking e/- char
                "(?<extra_ep_num>(?!(1080|720|480)[pi])\d+))*" &   ' additional E03/etc
                "[. _-]*((?<extra_info>.+?)" &                     ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                           ' Make sure this Is really the release group
                "-(?<release_group>[^- ]+))?)?$"                   ' Group
            )

            'fov
            '  Show_Name.1x02.Source_Quality_Etc-Group 
            '  Show Name - 1x02 - My Ep Name  
            '  Show_Name.1x02x03x04.Source_Quality_Etc-Group  
            '  Show Name - 1x02-03-04 - My Ep Name
            Patterns.Add("fov",
                "^((?<series_name>.+?)[\[. _-]+)?" &             ' Show_Name and separator
                "(?<season_num>\d+)x" &                          ' 1x
                "(?<ep_num>\d+)" &                               ' 02 and separator
                "(([. _-]*x|-)" &                                ' linking x/- char
                "(?<extra_ep_num>" &
                "(?!(1080|720|480)[pi])(?!(?<=x)264)" &          ' ignore obviously wrong multi-eps
                "\d+))*" &                                       ' additional x03/etc
                "[\]. _-]*((?<extra_info>.+?)" &                 ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                         ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                 ' Group"
            )

            'scene_date_format
            '  Show.Name.2010.11.23.Source.Quality.Etc-Group
            '  Show Name - 2010-11-23 - Ep Name
            Patterns.Add("scene_date_format",
                "^((?<series_name>.+?)[. _-]+)?" &               ' Show_Name And separator
                "(?<air_year>\d{4})[. _-]+" &                    ' 2010 And separator
                "(?<air_month>\d{2})[. _-]+" &                   ' 11 And separator
                "(?<air_day>\d{2})" &                            ' 23 And separator
                "[. _-]*((?<extra_info>.+?)" &                   ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                         ' Make sure this Is really the release group
                "-(?<release_group>[^- ]+))?)?$"                 ' Group
            )

            'stupid
            '  tpz-abc102
            Patterns.Add("stupid",
                "(?<release_group>.+?)-\w+?[\. ]?" &             ' tpz-abc
                "(?!264)" &                                      ' don't count x264
                "(?<season_num>\d{1,2})" &                       ' 1
                "(?<ep_num>\d{2})$"                              ' 02
            )

            'verbose
            '  Show Name Season 1 Episode 2 Ep Name
            Patterns.Add("verbose",
                "^(?<series_name>.+?)[. _-]+" &                  ' Show Name and separator
                "season[. _-]+" &                                ' season And separator
                "(?<season_num>\d+)[. _-]+" &                    ' 1
                "episode[. _-]+" &                               ' episode and separator
                "(?<ep_num>\d+)[. _-]+" &                        ' 02 and separator
                "(?<extra_info>.+)$"                             ' Source_Quality_Etc-"
            )

            'season_only
            '  Show.Name.S01.Source.Quality.Etc-Group
            Patterns.Add("season_only",
                "^((?<series_name>.+?)[. _-]+)?" &               ' Show_Name and separator
                "s(eason[. _-])?" &                              ' S01/Season 01
                "(?<season_num>\d+)[. _-]*" &                    ' S01 and optional separator
                "[. _-]*((?<extra_info>.+?)" &                   ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                         ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                 ' Group"
            )

            'bare
            '  Show.Name.102.Source.Quality.Etc-Group
            Patterns.Add("bare",
                "^(?<series_name>.+?)[. _-]+" &                      ' Show_Name and separator
                "(?<season_num>\d{1,2})" &                           ' 1
                "(?<ep_num>\d{2})" &                                 ' 02 and separator
                "([. _-]+(?<extra_info>(?!\d{3}[. _-]+)[^-]+)" &     ' Source_Quality_Etc-
                "(-(?<release_group>.+))?)?$"                        ' Group"
            )

            'no_season_multi_ep
            '  Show.Name.E02-03
            '  Show.Name.E02.2010
            Patterns.Add("no_season_multi_ep",
                "^((?<series_name>.+?)[. _-]+)?" &                              ' Show_Name and separator
                "(e(p(isode)?)?|part|pt)[. _-]?" &                              ' e, ep, episode, Or part
                "(?<ep_num>(\d+|[ivx]+))" &                                     ' first ep num
                "((([. _-]+(And|&|to)[. _-]+)|-)" &                             ' and/&/to joiner
                "(?<extra_ep_num>(?!(1080|720|480)[pi])(\d+|[ivx]+))[. _-])" &  ' second ep num
                "([. _-]*(?<extra_info>.+?)" &                                  ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                                        ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                                ' Group"
            )

            'no_season_general
            '  Show.Name.E23.Test
            '  Show.Name.Part.3.Source.Quality.Etc-Group
            '  Show.Name.Part.1.And.Part.2.Blah-Group
            Patterns.Add("no_season_general",
                "^((?<series_name>.+?)[. _-]+)?" &                   ' Show_Name and separator
                "(e(p(isode)?)?|part|pt)[. _-]?" &                   ' e, ep, episode, Or part
                "(?<ep_num>(\d+|([ivx]+(?=[. _-]))))" &              ' first ep num
                "([. _-]+((And|&|to)[. _-]+)?" &                     ' and/&/to joiner
                "((e(p(isode)?)?|part|pt)[. _-]?)" &                 ' e, ep, episode, or part
                "(?<extra_ep_num>(?!(1080|720|480)[pi])" &
                "(\d+|([ivx]+(?=[. _-]))))[. _-])*" &                ' second ep num
                "([. _-]*(?<extra_info>.+?)" &                       ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                             ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                     ' Group"
            )

            'no_season',
            '  Show Name - 01 - Ep Name
            '  01 - Ep Name
            Patterns.Add("no_season",
                "^((?<series_name>.+?)(?:[. _-]{2,}|[. _]))?" &      ' Show_Name and separator
                "(?<ep_num>\d{1,2})" &                               ' 01
                "(?:-(?<extra_ep_num>\d{1,2}))*" &                   ' 02
                "[. _-]+((?<extra_info>.+?)" &                       ' Source_Quality_Etc-
                "((?<![. _-])(?<!WEB)" &                             ' Make sure this is really the release group
                "-(?<release_group>[^- ]+))?)?$"                     ' Group"
            )

            'season_episode_only
            '  101
            Patterns.Add("season_episode_only",
                "(?<season_num>\d{1,2})" &                           ' 1
                "(?<ep_num>\d{2})"                                   ' 02 and separator
            )

            'Return Patterns
        End Function

    End Class

End Namespace
