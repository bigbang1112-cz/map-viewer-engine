using System.Runtime.InteropServices.JavaScript;

namespace MapViewerEngine.Modules;

internal static partial class Cam
{
    [JSImport("create", nameof(Cam))]
    internal static partial JSObject Create(double distance);

    [JSImport("move", nameof(Cam))]
    internal static partial void Move(double x, double y, double z);

    [JSImport("dispose", nameof(Cam))]
    internal static partial void Dispose();
}
