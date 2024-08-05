open FSharpTools.HttpRequests.Request
open FSharpTools.HttpRequests
open System
open FSharpTools


Client.initWithTimeout 16 <| TimeSpan.FromSeconds 5

let result = 
    callTest { Settings.getDefaultSettings () with Url = "http://pluto9:9090" } false   
    |> AsyncResult.toResult

async {
    let! affe = result
    printfn "%O" affe
} |> Async.RunSynchronously

