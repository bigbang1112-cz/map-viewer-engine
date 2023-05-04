using GbxToolAPI.Server;
using MapViewerEngine.Server.Models;
using MapViewerEngine.Server.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MapViewerEngine.Server.Endpoints.V1;

public class CollectionsEndpoint : IToolEndpoint
{
    public void Endpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("collections", GetAllCollections);
        app.MapGet("collections/{id}", GetCollection);
        app.MapPost("collections/add", AddCollection).RequireAuthorization(Policies.SuperAdminPolicy);
    }

    private async Task<IResult> GetAllCollections(ICollectionRepo repo, CancellationToken cancellationToken)
    {
        return Results.Ok(await repo.GetAllAsync(cancellationToken));
    }

    private async Task<IResult> GetCollection(ICollectionRepo repo, int id, CancellationToken cancellationToken)
    {
        var collection = await repo.GetByIdAsync(id, cancellationToken);

        if (collection is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(collection);
    }

    private async Task<IResult> AddCollection(IMapViewerEngineUnitOfWork uow, Collection collection, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collection.Name))
        {
            return Results.BadRequest("Name is required");
        }

        await uow.Collections.AddAsync(collection, cancellationToken);
        await uow.SaveAsync(cancellationToken);

        return Results.Created($"api/v1/map-viewer-engine/collections/{collection.Id}", collection);
    }
}
