using FlaxEngine;

namespace Tmore.SmartCamera;

/// <summary>
/// SmartVirtualCamera Script.
/// Attached to an empty actor representing a virtual camera
/// </summary>
public class SmartVirtualCamera : Script
{
    [Serialize]
    [Tooltip("Higher numbers are given more priority when changing cameras")]
    public int Priority { get; set; } = 0;

    [Header("Camera Properties")] 
    public float FieldOfView = 60;
    public float NearPlane = 10.0f;
    public float FarPlane = 1000f;
    public float AspectRatio = 1.778f;
    

    public Vector3 VirtualCameraPosition => Actor.Position;
    public Transform VirtualCameraTransform => Actor.Transform;
    
    public override void OnEnable()
    {
        Utils.LogMessage($"Virtual Camera: {Actor.Name} enabled");
        
        var cameraDirector = SmartCameraDirector.Instance;
        cameraDirector.AddVirtualCamera(this);
    }
    
    public override void OnDisable()
    {
        Utils.LogMessage($"Virtual Camera: {Actor.Name} disabled");
        
        var cameraDirector = SmartCameraDirector.Instance;
        cameraDirector.RemoveVirtualCamera(this);
    }
    
    internal virtual void UpdateVirtualCamera() {}
    internal virtual void FixedUpdateVirtualCamera() {}

    public override void OnDebugDrawSelected()
    {
        var frustum = BoundingFrustum.FromCamera(Actor.Position, Actor.Transform.Forward, Actor.Transform.Up,
            FieldOfView / 60f, NearPlane, FarPlane, AspectRatio);
        DebugDraw.DrawWireFrustum(frustum, Color.Red);
    }
}
