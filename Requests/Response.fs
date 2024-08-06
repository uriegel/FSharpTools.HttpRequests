namespace FSharpTools.HttpRequests
open System.Net.Http
open System.Net.Http.Json
open FSharpPlus
open FSharpTools.AsyncResult

module Response = 
    open System
    let asJson (res: AsyncResult<HttpResponseMessage, RequestError>) = 
        let asJson (msg: HttpResponseMessage) = 
            let mapException (exp: Exception) = 
                { Message = exp.Message; Type = Deserialization }

            let asJson () = 
                // TODO cancellationToken
                msg.Content.ReadFromJsonAsync<'a>()
            catch asJson
            |> AsyncResult.mapError mapException

        res >>= asJson
