﻿open FSharpTools.HttpRequests.Request
open FSharpTools.HttpRequests
open System
open FSharpTools
open FSharpTools.HttpRequests.Settings

Client.initWithTimeout 16 <| TimeSpan.FromSeconds 5

let call url = 
    request { Settings.getDefaultSettings () with Url = url } 
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

type LoginInput = {
    AndroidId: string
}

type LoginInputNotExisting = {
    AndroidIdNotExisting: string
}

type LoginOutput = {
    Registered: bool
}

type LoginOutputNotExisting = {
    RegisteredNotExisting: bool
}

let jsonPost<'a> (a: 'a) (url: string)  = 
    getDefaultSettings ()
    |> post
    |> Json.input<'a> a
    |> Settings.url url
    |> request

let ares = jsonPost { AndroidId = "Uwe Riegel" } "http://localhost:8080/superfit/login" 
// TODO HttpRequestException with status code and text
// TODO JsonSerializationError (in out)
                // InvalidOperationException ioe => new RequestInvalidOperationException(ioe),
                // TaskCanceledException => new TimeoutException(),
                // HttpRequestException hre when hre.InnerException is SocketException se && se.SocketErrorCode == SocketError.HostNotFound 
                //     => new HostNotFoundException(hre.Message),
                // HttpRequestException hre when hre.InnerException is SocketException se
                //     => new RequestSocketException(se),
                // Exception e => new HttpException(e.Message, e)
async {
    let! res = ares |> AsyncResult.toResult
    printfn "%A" res
} |> Async.RunSynchronously

let jsonRequest<'a, 'b> url a =
    getDefaultSettings ()
    |> post
    |> Json.input<'a> a
    |> Settings.url url
    |> request
    >>= Response.asJson<'b>

async {
    let! res = 
        jsonRequest "http://localhost:8080/superfit/login" { AndroidId = "Uwe Riegel" }
        |> AsyncResult.toResult
    match res with
    | Ok ok -> printfn "%b" ok.Registered
    | Error err -> printfn "%A" err
} |> Async.RunSynchronously

async {
    let! res = 
        jsonRequest "http://localhost:8080/superfit/login" { AndroidIdNotExisting = "Uwe Riegel" }
        |> AsyncResult.toResult
    match res with
    | Ok ok -> printfn "%b" ok.Registered
    | Error err -> printfn "%A" err
} |> Async.RunSynchronously

async {
    let! res = 
        jsonRequest "http://localhost:8080/superfit/login" { AndroidId = "Uwe Riegel" }
        |> AsyncResult.toResult
    match res with
    | Ok ok -> printfn "%b" ok.RegisteredNotExisting
    | Error err -> printfn "%A" err
} |> Async.RunSynchronously
