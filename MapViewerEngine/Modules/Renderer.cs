using System.Runtime.InteropServices.JavaScript;

namespace MapViewerEngine.Modules;

internal static partial class Renderer
{
    [JSImport("create", nameof(Renderer))]
    internal static partial JSObject Create();

    [JSImport("createScene", nameof(Renderer))]
    internal static partial JSObject CreateScene();

    [JSImport("addToScene", nameof(Renderer))]
    internal static partial JSObject AddToScene(JSObject obj);

    [JSImport("animate", nameof(Renderer))]
    internal static partial void Animate();

    [JSImport("spawnSampleObjects", nameof(Renderer))]
    internal static partial void SpawnSampleObjects();

    [JSImport("dispose", nameof(Renderer))]
    internal static partial void Dispose();
}
