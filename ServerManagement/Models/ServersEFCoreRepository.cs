using Microsoft.EntityFrameworkCore;
using ServerManagement.Data;

namespace ServerManagement.Models;

public class ServersEFCoreRepository : IServersEFCoreRepository
{
    private readonly IDbContextFactory<ServerManagementContext> contextFactory;

    public ServersEFCoreRepository(IDbContextFactory<ServerManagementContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public void AddServer(Server server)
    {
        using var db = contextFactory.CreateDbContext();
        db.Servers.Add(server);
        db.SaveChanges();
    }

    public List<Server> GetServers()
    {
        using var db = contextFactory.CreateDbContext();
        return db.Servers.AsNoTracking().ToList();
    }

    public List<Server> GetServersByCity(string cityName)
    {
        using var db = contextFactory.CreateDbContext();

        // Use Contains for SQLite compatibility instead of EF.Functions.Like
        return db.Servers
            .AsNoTracking()
            .Where(x => x.City != null && x.City.Contains(cityName))
            .ToList();
    }

    public Server? GetServerById(int id)
    {
        using var db = contextFactory.CreateDbContext();
        var server = db.Servers.AsNoTracking().FirstOrDefault(s => s.ServerId == id);
        return server ?? new Server();
    }

    public void UpdateServer(int serverId, Server server)
    {
        if (server == null) throw new ArgumentNullException(nameof(server));
        if (serverId != server.ServerId) return;

        using var db = contextFactory.CreateDbContext();
        var serverToUpdate = db.Servers.Find(serverId);
        if (serverToUpdate is not null)
        {
            serverToUpdate.IsOnline = server.IsOnline;
            serverToUpdate.Name = server.Name;
            serverToUpdate.City = server.City;

            db.SaveChanges();
        }
    }

    public void DeleteServer(int serverId)
    {
        using var db = contextFactory.CreateDbContext();
        var server = db.Servers.Find(serverId);
        if (server is null) return;

        db.Servers.Remove(server);
        db.SaveChanges();
    }

    public List<Server> SearchServers(string serverFilter)
    {
        using var db = contextFactory.CreateDbContext();

        // Use Contains for SQLite compatibility instead of EF.Functions.Like
        return db.Servers
            .AsNoTracking()
            .Where(x => x.Name != null && x.Name.Contains(serverFilter))
            .ToList();
    }
}