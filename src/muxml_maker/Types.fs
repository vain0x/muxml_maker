﻿namespace Muxml

[<AutoOpen>]
module Types =
  type LyricsText<'TInput, 'TTag> =
    | LyricsText of string

  type LyricsLine = {
      // 表示歌詞
      Show:  string
      // 入力歌詞
      Input: string
  }
  with
    static member Empty =
        { Show = "@"; Input = "@" }
    
    override this.ToString() =
        if this = LyricsLine.Empty
        then "@"
        else sprintf "%s[%s]" (this.Show) (this.Input)

  type TimeTag =
    | TimeTag of int
  with
    override this.ToString() =
        let (TimeTag ms) = this
        let min   = ms / (60 * 1000)
        let ms    = ms % (60 * 1000)
        let sec   = ms / 1000
        let ms    = ms % 1000
        let ms10  = ms / 10
        sprintf "[%02d:%02d:%02d]" min sec ms10

  type Interval =
    | Interval of int

  type OptionallyTimeTaggedList<'a> =
      ('a * TimeTag option * TimeTag option) list

  type TimeTaggedList<'a> =
      ('a * TimeTag * TimeTag) list

  type IntervalList<'a> =
      ('a * Interval) list

  type LyricsRepr<'a> =
    | WithTimeTag     of TimeTaggedList<'a>
    | WithInterval    of IntervalList<'a option>
  with
      member this.Shows() =
          match this with
          | WithTimeTag  xs -> xs |> List.map (fun (show, _, _) -> show)
          | WithInterval xs -> xs |> List.choose fst

  // 入力歌詞が未設定な歌詞データ
  type UnreadableLyrics =
      LyricsRepr<string>

  type Lyrics =
      LyricsRepr<LyricsLine>

  type MetaData = {
      Name            : string
      MusicPath       : string
      PicPath         : string option
      VideoPath       : string option
      Artist          : string option
      Genre           : string option
  }
