using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ServerManagement.Models;

namespace ServerManagement.StateStore;

public class SessionStorage
{
    private readonly ProtectedSessionStorage protectedSessionStorage;

    public SessionStorage(ProtectedSessionStorage protectedSessionStorage)
    {
        this.protectedSessionStorage = protectedSessionStorage;
    }

    public async Task<Server?> GetServerAsync()
    {
        var result = await protectedSessionStorage.GetAsync<Server>("server");
        if (result.Success)
            return result.Value;
        return null;
    }

    public async Task SetServerAsync(Server? server)
    {
        await protectedSessionStorage.SetAsync("server", server);
    }
}