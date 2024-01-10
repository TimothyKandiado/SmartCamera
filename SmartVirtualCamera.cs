using FlaxEngine;

namespace Tmore.SmartCam;

/// <summary>
/// SmartVirtualCam Script.
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
        var cameraDirector = SmartCameraDirector.Instance;
        cameraDirector.AddVirtualCamera(this);
    }
    
    public override void OnDisable()
    {
        var cameraDirector = SmartCameraDirector.Instance;
        cameraDirector.RemoveVirtualCamera(this);
    }
    
    internal virtual void UpdateVirtualCamera()
    {
        
    }

    public override void OnDebugDrawSelected()
    {
        var frustum = BoundingFrustum.FromCamera(Actor.Position, Actor.Transform.Forward, Actor.Transform.Up,
            FieldOfView / 60f, NearPlane, FarPlane, AspectRatio);
        DebugDraw.DrawWireFrustum(frustum, Color.Red);
    }
}
