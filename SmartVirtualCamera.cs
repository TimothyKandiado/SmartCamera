using System;
using System.Collections.Generic;
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
}
