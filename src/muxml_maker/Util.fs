﻿[<AutoOpen>]
module Util

[<AutoOpen>]
module Trivial =
  let tap f x =
      do f x
      x

module Option =
  let if' b f =
      if b then Some (f ()) else None

  let filter pred self =
      self |> Option.bind (fun it -> if' (pred it) (fun () -> it))

module List =
  // not used
  let rec tryLast = function
    | [] -> None
    | [x] -> Some x
    | x :: xs -> tryLast xs

  let rec dropLast = function
    | [] | [_] -> []
    | x :: xs -> x :: dropLast xs

  /// xs に、各要素の前後の情報を加えたリスト
  let zipPrevNext front back prev next xs =
      let prevs = front :: (xs |> dropLast |> List.map prev)
      let nexts = (xs |> List.tail |> List.map next) @ [back]
      List.zip3 xs prevs nexts
