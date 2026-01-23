namespace TeamChoice.WebApis.Domain.Models;

public class ApiResponse
{
    public string TimeStamp { get; set; }
    public string Status { get; set; }
    public int StatusCode { get; set; }
    public object Data { get; set; }
    public string Message { get; set; }
}
