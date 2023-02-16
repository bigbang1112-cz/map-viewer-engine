using GbxToolAPI.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace MapViewerEngine;

public partial class MapViewerEngineHub : ToolHub
{
    public MapViewerEngineHub(string baseAddress, ILogger<ToolHub>? logger = null) : base(baseAddress, logger)
    {
    }

    public async Task<byte[]> SendBlockMesh(string blockName,
                                              bool ground,
                                              int variant,
                                              int subVariant,
                                              CancellationToken cancellationToken = default)
    {
        return await Connection.InvokeAsync<byte[]>(nameof(SendBlockMesh), blockName, ground, variant, subVariant, cancellationToken);
    }
}
