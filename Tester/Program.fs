open FSharpTools.HttpRequests.Request
open FSharpTools.HttpRequests
open System
open FSharpTools

Client.initWithTimeout 16 <| TimeSpan.FromSeconds 5

let call url = 
    callTest { Settings.getDefaultSettings () with Url = url } false   
    |> AsyncResult.toResult

async {
    let! affe = call "http://pluto9:9090"
    printfn "%O" affe
} |> Async.RunSynchronously

async {
    let! affe = call "http://pluto:9090"
    printfn "%O" affe
} |> Async.RunSynchronously

async {
    let! affe = call "http://pluto:8080/notavaliable"
    printfn "%O" affe
} |> Async.RunSynchronously

async {
    let! affe = call "http://pluto:8080/getfiles/home/uwe"
    printfn "%O" affe
} |> Async.RunSynchronously

// TODO HttpRequestException with status code and text
// TODO certain status codes are not errors // controllable
// TODO NameResolutionError
// TODO Connection refused error
// TODO Unknown exceptions
// TODO JsonSerializationError (in out)
                // InvalidOperationException ioe => new RequestInvalidOperationException(ioe),
                // TaskCanceledException => new TimeoutException(),
                // HttpRequestException hre when hre.InnerException is SocketException se && se.SocketErrorCode == SocketError.HostNotFound 
                //     => new HostNotFoundException(hre.Message),
                // HttpRequestException hre when hre.InnerException is SocketException se
                //     => new RequestSocketException(se),
                // Exception e => new HttpException(e.Message, e)
