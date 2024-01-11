using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Tmore.SmartCamera;

/// <summary>
/// SmartCameraDirector Script.
/// Needs to be attached to a Camera
/// </summary>

public class SmartCameraDirector : Script
{
    private readonly List<SmartVirtualCamera> _virtualCameras = [];
    private SmartVirtualCamera _currentVirtualCamera;

    internal static SmartCameraDirector Instance => _instance;

    private SmartCameraDirector() {}

    private static SmartCameraDirector _instance;

    public override void OnAwake() {
        if (_instance)
        {
            Destroy(this);
            return;
        }
        
        _instance = this;
    }
    
    public override void OnUpdate()
    {
        if (!_currentVirtualCamera) return;
        
        _currentVirtualCamera.UpdateVirtualCamera();
    }

    public override void OnFixedUpdate()
    {
        if (!_currentVirtualCamera) return;
        
        _currentVirtualCamera.FixedUpdateVirtualCamera();
        
        Actor.Position = _currentVirtualCamera.VirtualCameraPosition;
        Actor.Rotation = _currentVirtualCamera.Transform.GetRotation();
    }

    internal void AddVirtualCamera(SmartVirtualCamera virtualCamera)
    {
        if (_virtualCameras.Contains(virtualCamera)) return;
        
        Utils.LogMessage($"Virtual Camera: {virtualCamera.Actor.Name} added");
        _virtualCameras.Add(virtualCamera);
        Flush();
    }

    internal void RemoveVirtualCamera(SmartVirtualCamera virtualCamera)
    {
        _virtualCameras.Remove(virtualCamera);
        Utils.LogMessage($"Virtual Camera: {virtualCamera.Actor.Name} removed");
        Utils.LogMessage(_virtualCameras);
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

    public override void OnDestroy()
    {
        _virtualCameras.Clear();
        _instance = null;
        
        Utils.LogMessage("Destroying Camera Director");
    }
}
