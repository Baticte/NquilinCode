namespace NquilinCode.Communication.Responses;

public class ResponseErrorJson
{
    public IReadOnlyList<string> Errors { get; set; }
    public ResponseErrorJson(IReadOnlyList<string> errors)
    {
        Errors = errors;
    }

    public ResponseErrorJson(string error)
    {
        Errors = new List<string>() { error };
    }
}