using System.Text;
using SurrealDb.Net;
using SurrealDb.Net.Models;

namespace Acascendia.Models;

public class Chat : Record
{
    public string? Name {get; set;}
    public List<string>? Users {get; set;}

    public async Task<string> GetName(SurrealDbClient client, string userId)
    {
        if (Name is null) {
            var other = Users.First();
            if (other == userId) {
                other = Users[1];
                var res = await client.Select<User>(("user", other));
                if (res is not null) {
                    other = res.Username;
                }
            }
            return other;
        }else
        {
            return Name;
        }
    }
}



public class Message : Record
{
    public string? Parent {get; set;}
    public DateTime? Date {get; set;}
    public string? User {get; set;}
    public string? Text {get; set;}
}