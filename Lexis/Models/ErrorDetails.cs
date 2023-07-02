using Newtonsoft.Json;

namespace LexisApi.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = null!;

    public string Details { get; set; } = null!;

    public IEnumerable<string> InvalidProperties { get; set; } = null!;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}