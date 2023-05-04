using System.Runtime.InteropServices.JavaScript;

namespace MapViewerEngine.Modules;

public static partial class Cam
{
    [JSImport("create", nameof(Cam))]
    internal static partial JSObject Create(double distance);

    [JSImport("getCam", nameof(Cam))]
    public static partial JSObject Get();

    [JSImport("getCamTarget", nameof(Cam))]
    public static partial JSObject GetTarget();

    [JSImport("setCamTarget", nameof(Cam))]
    public static partial JSObject SetTarget(double x, double y, double z, bool smooth = false);

    [JSImport("changeDistance", nameof(Cam))]
    public static partial void ChangeDistance(double distance);

    [JSImport("lockTo", nameof(Cam))]
    public static partial void LockTo(JSObject obj);

    [JSImport("dispose", nameof(Cam))]
    internal static partial void Dispose();
}
