using GBX.NET.Engines.Game;

namespace MapViewerEngine;

public class MapViewerAPI
{
    private readonly CGameCtnChallenge map;

    public MapViewerAPI(CGameCtnChallenge map)
	{
        this.map = map;
    }
}
