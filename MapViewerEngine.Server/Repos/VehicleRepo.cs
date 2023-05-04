using GbxToolAPI.Server.Options;
using GbxToolAPI.Server;
using MapViewerEngine.Shared;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace MapViewerEngine.Server.Repos;

public interface IVehicleRepo
{
    Task<VehicleData?> GetDataAsync(string vehicleName, CancellationToken cancellationToken = default);
}

public class VehicleRepo : IVehicleRepo
{
    private readonly MapViewerEngineContext context;
    private readonly ISqlConnection<MapViewerEngineServer> sql;
    private readonly IOptions<DatabaseOptions> options;

    private bool IsInMemory => options.Value.InMemory;

    public VehicleRepo(MapViewerEngineContext context, ISqlConnection<MapViewerEngineServer> sql, IOptions<DatabaseOptions> options)
    {
        this.context = context;
        this.sql = sql;
        this.options = options;
    }
    
    public async Task<VehicleData?> GetDataAsync(string vehicleName, CancellationToken cancellationToken = default)
    {
        _ = vehicleName ?? throw new ArgumentNullException(nameof(vehicleName));

        if (IsInMemory)
        {
            return await context.Vehicles
                .Include(v => v.Mesh)
                .Where(v => v.Name == vehicleName)
                .Select(v => new VehicleData(v.Mesh.Data))
                .FirstOrDefaultAsync(cancellationToken);
        }

        return await sql.Connection.QueryFirstOrDefaultAsync<VehicleData?>(
            @"SELECT Meshes.Data FROM Vehicles
              INNER JOIN Meshes ON Vehicles.MeshId = Meshes.Id
              WHERE Vehicles.Name = @Name", new { Name = vehicleName });
    }
}
