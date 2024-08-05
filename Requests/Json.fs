namespace FSharpTools.HttpRequests
open System.Net.Http.Json

module Json = 
    open System.Net.Http
    
    let input<'a> (a: 'a) (settings: Settings) = 
        let getJson () : HttpContent = 
            JsonContent.Create a
            
        { settings with AddContent = Some getJson }