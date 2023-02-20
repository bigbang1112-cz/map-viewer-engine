using GbxToolAPI.Server;
using Mapster;
using MapViewerEngine.Server.Models.Dtos;
using MapViewerEngine.Server.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MapViewerEngine.Server.Endpoints.V1;

public class OfficialBlocksEndpoint : IToolEndpoint
{
    public void Endpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("official-blocks/{author}/{collection}/{name}", GetOfficialBlockByIdent);
    }

    private async Task<IResult> GetOfficialBlockByIdent(string author, string collection, string name, IOfficialBlockRepo repo, CancellationToken cancellationToken)
    {
        var officialBlock = await repo.GetByIdentAsync(author, collection, name, cancellationToken);

        if (officialBlock is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(officialBlock.Adapt<OfficialBlockDto>());
    }
}
