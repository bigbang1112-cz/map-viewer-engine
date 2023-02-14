using GBX.NET.Engines.Game;
using GbxToolAPI;

namespace MapViewerEngine;

public class MapViewerEngineTool : ITool
{
    private readonly CGameCtnChallenge map;

    public MapViewerEngineTool(CGameCtnChallenge map)
	{
        this.map = map;
    }
}
