﻿open FSharpTools.HttpRequests.Request
open FSharpTools.HttpRequests
open System
open FSharpTools

Client.initWithTimeout 16 <| TimeSpan.FromSeconds 5

let call url = 
    callTest { Settings.getDefaultSettings () with Url = url } false   
    |> AsyncResult.toResult

async {
    let! res = call "http://nichtda:9090"
    printfn "%A" res
} |> Async.RunSynchronously

async {
    let! res = call "http://pluto:9090"
    printfn "%A" res
} |> Async.RunSynchronously

async {
    let! res = call "http://pluto:8080/notavaliable"
    printfn "%A" res
} |> Async.RunSynchronously

async {
    let! res = call "http://pluto:8080/getfiles/home/uwe"
    printfn "%A" res
} |> Async.RunSynchronously

// TODO HttpRequestException with status code and text
// TODO JsonSerializationError (in out)
                // InvalidOperationException ioe => new RequestInvalidOperationException(ioe),
                // TaskCanceledException => new TimeoutException(),
                // HttpRequestException hre when hre.InnerException is SocketException se && se.SocketErrorCode == SocketError.HostNotFound 
                //     => new HostNotFoundException(hre.Message),
                // HttpRequestException hre when hre.InnerException is SocketException se
                //     => new RequestSocketException(se),
                // Exception e => new HttpException(e.Message, e)
