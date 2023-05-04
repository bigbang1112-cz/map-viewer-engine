using System.Runtime.InteropServices.JavaScript;

namespace MapViewerEngine.Modules;

public partial class Timeline
{
    [JSImport("create", nameof(Timeline))]
    public static partial JSObject Create(JSObject[] animations);
    
    [JSImport("createAnimation", nameof(Timeline))]
    public static partial JSObject CreateAnimation(JSObject target, JSObject[] keyframes);

    [JSImport("createKeyframeVec", nameof(Timeline))]
    public static partial JSObject CreateKeyframeVec(int duration, double x, double y, double z);

    [JSImport("createKeyframeQuat", nameof(Timeline))]
    public static partial JSObject CreateKeyframeQuat(int duration, double x, double y, double z, double w);

    [JSImport("play", nameof(Timeline))]
    public static partial void Play(JSObject timeline);

    [JSImport("pause", nameof(Timeline))]
    public static partial void Pause(JSObject timeline);
}
