﻿namespace Muxml

open System.Text

module Converter =
  open Converter

  module XmlFromLrc =
    let xml_from_lyrics = function
      | WithInterval ls ->
          let len         = ls |> List.length
          let sh_words    = StringBuilder()
          let in_words    = StringBuilder()
          let intervals   = StringBuilder()

          for (line_opt, Interval interval) in ls do
            let line =
                match line_opt with
                | Some line -> line
                | None -> LyricsLine.Empty

            sh_words .AppendLine(sprintf "<word>%s</word>" (line.Show)) |> ignore
            in_words .AppendLine(sprintf "<nihongoword>%s</nihongoword>" (line.Input)) |> ignore
            intervals.AppendLine(sprintf "<interval>%d</interval>\n" (interval)) |> ignore

          (string sh_words) + (string in_words) + (string intervals)
      | _ ->
          failwith "Unimplemented"

    ()

  open XmlFromLrc

  let to_xml (data: MetaData) (lyrics: Lyrics) =
    let lyrics_elems = lyrics |> xml_from_lyrics

    let kpm_elem =
        ""
        //"<kpm>" + ModelKPM.ToString("f2") + "</kpm>"

    let video_elem =
        ""
        //(Video.Uses ? "<video src=\"" + Video.GetFullPath() + "\" scalemode=\"fullwidth\" />\n" : "")

    let pic_elem =
        ""//(Pic.Uses   ? "<background id=\"" + Pic.GetFullPath() + "\" />\n" : "")

    ( "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>\n"
    + "<musicXML>\n"
    + video_elem
    + pic_elem
    + "<musicname>" + data.Name + "</musicname>\n"
    + lyrics_elems
    + "</musicXML>\n"
    )
