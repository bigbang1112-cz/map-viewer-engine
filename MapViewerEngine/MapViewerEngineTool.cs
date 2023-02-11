using GBX.NET.Engines.Game;
using GbxToolAPI;

namespace MapViewerEngine;

public class MapViewerEngineTool : Tool
{
    private readonly CGameCtnChallenge map;

    public MapViewerEngineTool(CGameCtnChallenge map)
	{
        this.map = map;
    }
}
