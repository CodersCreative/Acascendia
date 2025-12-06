using SurrealDb.Net.Models;
namespace Acascendia.Models;

public class Class : Record
{
    public string? Name {get; set;}
    public List<string>? Users {get; set;}
    public List<string>? Teachers {get; set;}
    public string? Code {get; set;}
}

public class Assignment : Record
{
    public string? Name {get; set;}
    public string? Class {get; set;}
    public DateTime? Due {get; set;}
    public string? Text {get; set;}
}

public class Submission : Record
{
    public string? Assignment {get; set;}
    public DateTime? Date {get; set;}
    public string? User {get; set;}
    public string? Text {get; set;}
}