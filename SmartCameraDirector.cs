using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Tmore.SmartCam;

/// <summary>
/// SmartVirtualCam Script.
/// </summary>

public class SmartCameraDirector : Script
{
    public Camera MainCamera;
    
    private readonly List<SmartVirtualCamera> _virtualCameras = [];
    private SmartVirtualCamera _currentVirtualCamera;
    
    internal static SmartCameraDirector Instance
    {
        get
        {
            if (_instance) return _instance;

            var actor = new EmptyActor
            {
                Name = "SmartCamDirector"
            };
            
            var script = actor.AddScript<SmartCameraDirector>();
            _instance = script;
            return _instance;
        }
    }

    private SmartCameraDirector() {}

    private static SmartCameraDirector _instance;

    public override void OnAwake() {
        if (_instance)
        {
            Destroy(this);
            return;
        }
        
        _instance = this;
        
        if (MainCamera) return;
        
        MainCamera = Camera.MainCamera;
    }
    
    public override void OnUpdate()
    {
        if (!_currentVirtualCamera) return;
        
        _currentVirtualCamera.UpdateVirtualCamera();
        
        Actor.Position = _currentVirtualCamera.VirtualCameraPosition;
        Actor.Rotation = _currentVirtualCamera.Transform.GetRotation();
    }

    internal void AddVirtualCamera(SmartVirtualCamera virtualCamera)
    {
        if (_virtualCameras.Contains(virtualCamera)) return;
        
        _virtualCameras.Add(virtualCamera);
    }

    internal void RemoveVirtualCamera(SmartVirtualCamera virtualCamera)
    {
        if (!_virtualCameras.Contains(virtualCamera)) return;
        
        _virtualCameras.Remove(virtualCamera);
        Flush();
    }
    
    private void Flush()
    {
        if (_virtualCameras.Count == 0) return;

        if (!_virtualCameras.Contains(_currentVirtualCamera))
        {
            _currentVirtualCamera = GetPriorityVirtualCamera();
        }
    }

    /// <summary>
    /// Return the highest priority camera available
    /// Only call when number of virtual cameras is not zero
    /// </summary>
    /// <returns>SmartVirtualCamera</returns>
    private SmartVirtualCamera GetPriorityVirtualCamera()
    {
        SmartVirtualCamera priorityCamera = null;
        var currentPriority = int.MinValue;

        foreach (var virtualCamera in _virtualCameras)
        {
            if (virtualCamera.Priority < currentPriority) continue;

            priorityCamera = virtualCamera;
            currentPriority = virtualCamera.Priority;
        }

        return priorityCamera;
    }
}
