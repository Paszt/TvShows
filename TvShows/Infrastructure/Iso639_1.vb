Public NotInheritable Class Iso639_1

    Public Shared Function GetName(isoCode As Code) As String
        Dim result As String = String.Empty
        CodeDictionary.TryGetValue(isoCode.ToString(), result)
        Return result
    End Function

    Public Shared CodeDictionary As New Dictionary(Of String, String)() From {
        {"aa", "Afar"},
        {"ab", "Abkhazian"},
        {"ae", "Avestan"},
        {"af", "Afrikaans"},
        {"ak", "Akan"},
        {"am", "Amharic"},
        {"an", "Aragonese"},
        {"ar", "Arabic"},
        {"as", "Assamese"},
        {"av", "Avaric"},
        {"ay", "Aymara"},
        {"az", "Azerbaijani"},
        {"ba", "Bashkir"},
        {"be", "Belarusian"},
        {"bg", "Bulgarian"},
        {"bh", "Bihari languages"},
        {"bi", "Bislama"},
        {"bm", "Bambara"},
        {"bn", "Bengali"},
        {"bo", "Tibetan"},
        {"br", "Breton"},
        {"bs", "Bosnian"},
        {"ca", "Catalan; Valencian"},
        {"ce", "Chechen"},
        {"ch", "Chamorro"},
        {"co", "Corsican"},
        {"cr", "Cree"},
        {"cs", "Czech"},
        {"cu", "Church Slavic; Old Slavonic; Church Slavonic; Old Bulgarian; Old Church Slavonic"},
        {"cv", "Chuvash"},
        {"cy", "Welsh"},
        {"da", "Danish"},
        {"de", "German"},
        {"dv", "Divehi; Dhivehi; Maldivian"},
        {"dz", "Dzongkha"},
        {"ee", "Ewe"},
        {"el", "Greek, Modern (1453-)"},
        {"en", "English"},
        {"eo", "Esperanto"},
        {"es", "Spanish; Castilian"},
        {"et", "Estonian"},
        {"eu", "Basque"},
        {"fa", "Persian"},
        {"ff", "Fulah"},
        {"fi", "Finnish"},
        {"fj", "Fijian"},
        {"fo", "Faroese"},
        {"fr", "French"},
        {"fy", "Western Frisian"},
        {"ga", "Irish"},
        {"gd", "Gaelic; Scottish Gaelic"},
        {"gl", "Galician"},
        {"gn", "Guarani"},
        {"gu", "Gujarati"},
        {"gv", "Manx"},
        {"ha", "Hausa"},
        {"he", "Hebrew"},
        {"hi", "Hindi"},
        {"ho", "Hiri Motu"},
        {"hr", "Croatian"},
        {"ht", "Haitian; Haitian Creole"},
        {"hu", "Hungarian"},
        {"hy", "Armenian"},
        {"hz", "Herero"},
        {"ia", "Interlingua (International Auxiliary Language Association)"},
        {"id", "Indonesian"},
        {"ie", "Interlingue; Occidental"},
        {"ig", "Igbo"},
        {"ii", "Sichuan Yi; Nuosu"},
        {"ik", "Inupiaq"},
        {"io", "Ido"},
        {"is", "Icelandic"},
        {"it", "Italian"},
        {"iu", "Inuktitut"},
        {"ja", "Japanese"},
        {"jv", "Javanese"},
        {"ka", "Georgian"},
        {"kg", "Kongo"},
        {"ki", "Kikuyu; Gikuyu"},
        {"kj", "Kuanyama; Kwanyama"},
        {"kk", "Kazakh"},
        {"kl", "Kalaallisut; Greenlandic"},
        {"km", "Central Khmer"},
        {"kn", "Kannada"},
        {"ko", "Korean"},
        {"kr", "Kanuri"},
        {"ks", "Kashmiri"},
        {"ku", "Kurdish"},
        {"kv", "Komi"},
        {"kw", "Cornish"},
        {"ky", "Kirghiz; Kyrgyz"},
        {"la", "Latin"},
        {"lb", "Luxembourgish; Letzeburgesch"},
        {"lg", "Ganda"},
        {"li", "Limburgan; Limburger; Limburgish"},
        {"ln", "Lingala"},
        {"lo", "Lao"},
        {"lt", "Lithuanian"},
        {"lu", "Luba-Katanga"},
        {"lv", "Latvian"},
        {"mg", "Malagasy"},
        {"mh", "Marshallese"},
        {"mi", "Maori"},
        {"mk", "Macedonian"},
        {"ml", "Malayalam"},
        {"mn", "Mongolian"},
        {"mr", "Marathi"},
        {"ms", "Malay"},
        {"mt", "Maltese"},
        {"my", "Burmese"},
        {"na", "Nauru"},
        {"nb", "Bokmål, Norwegian; Norwegian Bokmål"},
        {"nd", "Ndebele, North; North Ndebele"},
        {"ne", "Nepali"},
        {"ng", "Ndonga"},
        {"nl", "Dutch; Flemish"},
        {"nn", "Norwegian Nynorsk; Nynorsk, Norwegian"},
        {"no", "Norwegian"},
        {"nr", "Ndebele, South; South Ndebele"},
        {"nv", "Navajo; Navaho"},
        {"ny", "Chichewa; Chewa; Nyanja"},
        {"oc", "Occitan (post 1500); Provençal"},
        {"oj", "Ojibwa"},
        {"om", "Oromo"},
        {"or", "Oriya"},
        {"os", "Ossetian; Ossetic"},
        {"pa", "Panjabi; Punjabi"},
        {"pi", "Pali"},
        {"pl", "Polish"},
        {"ps", "Pushto; Pashto"},
        {"pt", "Portuguese"},
        {"qu", "Quechua"},
        {"rm", "Romansh"},
        {"rn", "Rundi"},
        {"ro", "Romanian; Moldavian; Moldovan"},
        {"ru", "Russian"},
        {"rw", "Kinyarwanda"},
        {"sa", "Sanskrit"},
        {"sc", "Sardinian"},
        {"sd", "Sindhi"},
        {"se", "Northern Sami"},
        {"sg", "Sango"},
        {"si", "Sinhala; Sinhalese"},
        {"sk", "Slovak"},
        {"sl", "Slovenian"},
        {"sm", "Samoan"},
        {"sn", "Shona"},
        {"so", "Somali"},
        {"sq", "Albanian"},
        {"sr", "Serbian"},
        {"ss", "Swati"},
        {"st", "Sotho, Southern"},
        {"su", "Sundanese"},
        {"sv", "Swedish"},
        {"sw", "Swahili"},
        {"ta", "Tamil"},
        {"te", "Telugu"},
        {"tg", "Tajik"},
        {"th", "Thai"},
        {"ti", "Tigrinya"},
        {"tk", "Turkmen"},
        {"tl", "Tagalog"},
        {"tn", "Tswana"},
        {"to", "Tonga (Tonga Islands)"},
        {"tr", "Turkish"},
        {"ts", "Tsonga"},
        {"tt", "Tatar"},
        {"tw", "Twi"},
        {"ty", "Tahitian"},
        {"ug", "Uighur; Uyghur"},
        {"uk", "Ukrainian"},
        {"ur", "Urdu"},
        {"uz", "Uzbek"},
        {"ve", "Venda"},
        {"vi", "Vietnamese"},
        {"vo", "Volapük"},
        {"wa", "Walloon"},
        {"wo", "Wolof"},
        {"xh", "Xhosa"},
        {"yi", "Yiddish"},
        {"yo", "Yoruba"},
        {"za", "Zhuang; Chuang"},
        {"zh", "Chinese"},
        {"zu", "Zulu"}
    }

    Public Enum Code As Integer
        aa = 1
        ab = 2
        ae = 3
        af = 4
        ak = 5
        am = 6
        an = 7
        ar = 8
        [as] = 9
        av = 10
        ay = 11
        az = 12
        ba = 13
        be = 14
        bg = 15
        bh = 16
        bi = 17
        bm = 18
        bn = 19
        bo = 20
        br = 21
        bs = 22
        ca = 23
        ce = 24
        ch = 25
        co = 26
        cr = 27
        cs = 28
        cu = 29
        cv = 30
        cy = 31
        da = 32
        de = 33
        dv = 34
        dz = 35
        ee = 36
        el = 37
        en = 38
        eo = 39
        es = 40
        et = 41
        eu = 42
        fa = 43
        ff = 44
        fi = 45
        fj = 46
        fo = 47
        fr = 48
        fy = 49
        ga = 50
        gd = 51
        gl = 52
        gn = 53
        gu = 54
        gv = 55
        ha = 56
        he = 57
        hi = 58
        ho = 59
        hr = 60
        ht = 61
        hu = 62
        hy = 63
        hz = 64
        ia = 65
        id = 66
        ie = 67
        ig = 68
        ii = 69
        ik = 70
        io = 71
        [is] = 72
        it = 73
        iu = 74
        ja = 75
        jv = 76
        ka = 77
        kg = 78
        ki = 79
        kj = 80
        kk = 81
        kl = 82
        km = 83
        kn = 84
        ko = 85
        kr = 86
        ks = 87
        ku = 88
        kv = 89
        kw = 90
        ky = 91
        la = 92
        lb = 93
        lg = 94
        li = 95
        ln = 96
        lo = 97
        lt = 98
        lu = 99
        lv = 100
        mg = 101
        mh = 102
        mi = 103
        mk = 104
        ml = 105
        mn = 106
        mr = 107
        ms = 108
        mt = 109
        [my] = 110
        na = 111
        nb = 112
        nd = 113
        ne = 114
        ng = 115
        nl = 116
        nn = 117
        no = 118
        nr = 119
        nv = 120
        ny = 121
        oc = 122
        oj = 123
        om = 124
        [or] = 125
        os = 126
        pa = 127
        pi = 128
        pl = 129
        ps = 130
        pt = 131
        qu = 132
        rm = 133
        rn = 134
        ro = 135
        ru = 136
        rw = 137
        sa = 138
        sc = 139
        sd = 140
        se = 141
        sg = 142
        si = 143
        sk = 144
        sl = 145
        sm = 146
        sn = 147
        so = 148
        sq = 149
        sr = 150
        ss = 151
        st = 152
        su = 153
        sv = 154
        sw = 155
        ta = 156
        te = 157
        tg = 158
        th = 159
        ti = 160
        tk = 161
        tl = 162
        tn = 163
        [to] = 164
        tr = 165
        ts = 166
        tt = 167
        tw = 168
        ty = 169
        ug = 170
        uk = 171
        ur = 172
        uz = 173
        ve = 174
        vi = 175
        vo = 176
        wa = 177
        wo = 178
        xh = 179
        yi = 180
        yo = 181
        za = 182
        zh = 183
        zu = 184
    End Enum

End Class
