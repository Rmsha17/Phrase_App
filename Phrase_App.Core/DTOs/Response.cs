public class Response
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Response SuccessResponse(string message) =>
        new Response { Success = true, Message = message };

    public static Response FailResponse(string message) =>
        new Response { Success = false, Message = message };
}
