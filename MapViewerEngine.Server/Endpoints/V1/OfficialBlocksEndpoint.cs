using GbxToolAPI.Server;
using MapViewerEngine.Server.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MapViewerEngine.Server.Endpoints.V1;

public class OfficialBlocksEndpoint : IToolEndpoint
{
    public void Endpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("official-blocks", OfficialBlocks)
            .RequireAuthorization(Policies.SuperAdminPolicy);
    }

    private async Task<IResult> OfficialBlocks(IOfficialBlockRepo repo, CancellationToken cancellationToken)
    {
        return Results.Ok(await repo.GetAllAsync(cancellationToken));
    }
}
