namespace NquilinCode.Communication.Responses;

public class ResponseErrorJson
{
    public IReadOnlyList<string> Errors { get; set; }
    public ResponseErrorJson(IReadOnlyList<string> errors) => Errors = errors;
    
    public bool TokenIsExpired { get; set; }

    public ResponseErrorJson(string error)
    {
        Errors = new List<string>() { error };
    }
}