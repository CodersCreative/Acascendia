using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using studbud.Shared;
using studbud.Shared.Models;

namespace studbud.Client.Shared;

public class HubService
{
    Blazored.SessionStorage.ISessionStorageService sessionStorage;
    NavigationManager navManager;

    public HubService (Blazored.SessionStorage.ISessionStorageService sessionStorage, NavigationManager navManager)
    {
        this.sessionStorage = sessionStorage;
        this.navManager = navManager;
    }
    public IHubConnectionBuilder GetHubConnection()
    {
        return new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl(navManager.ToAbsoluteUri("/apphub"))
            .AddJsonProtocol(cfg =>
            {
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                jsonOptions.Converters.Add(new JsonStringEnumConverter());

                cfg.PayloadSerializerOptions = jsonOptions;
            });
    }

    public async Task<User?> AutoLogin(HubConnection hub)
    {
        var user = await sessionStorage.GetItemAsync<User>("user");
        if (user is not null)
        {
            var check = await hub.InvokeAsync<User?>(nameof(IAppHubServer.CheckUser), user.id);
            await sessionStorage.SetItemAsync("user", check);
            return check;
        }
        else
        {
            return null;
        }
    }
}
