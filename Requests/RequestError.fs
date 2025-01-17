namespace FSharpTools.HttpRequests
open System

type ErrorType =
    | Unknown of Exception
    | HostNotFound
    | ConnectionRefused
    | Socket
    | Timeout
    | Deserialization
    | Http of int

type RequestError = {
    Message: string
    Type: ErrorType
}

