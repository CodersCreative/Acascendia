using SurrealDb.Net.Models;

namespace Acascendia.Models;

public class Class : Record
{
    public string? Name {get; set;}
    public List<string>? Users {get; set;}
    public List<string>? Teachers {get; set;}
    public string? Code {get; set;}
}