using System;
using FlaxEngine;
using Tmore.MathUtils;

namespace Tmore.SmartCamera;

[Serializable]
public struct Rig
{
    public float Radius;
    public float Height;
}

public class SmartOrbitCamera : SmartVirtualCamera
{
    [Header("Orbit Settings")]
    [Space(2)]
    [Serialize] private Rig TopRig = new() {Radius = 10, Height = 15};
    [Serialize] private Rig MiddleRig = new() {Radius = 20, Height = 3};
    [Serialize] private Rig BottomRig = new() {Radius = 8, Height = -10};
    [Range(0, 360)] 
    public float OrbitAngle = 0;

    [Range(-1, 1)] 
    [Tooltip("Position of camera relative to rigs, -1 for bottom rig, 1 for top rig")]
    public float VerticalPosition = 0;
    
    [Space(2)] 
    public Actor Target;

    #region Private
    private float _trueVerticalCameraPosition = 0;
    private Vector3 _orbitPosition;
    private CubicSpline _spline;
    #endregion

    public override void OnStart()
    {
        CalculateSpline();
    }

    internal override void FixedUpdateVirtualCamera()
    {
        MoveCamera();
    }

    public override void OnDebugDrawSelected()
    {
        if (Target) DrawOrbitGizmos();
        
        base.OnDebugDrawSelected();
    }

    private void DrawOrbitGizmos()
    {
        DrawRig(TopRig);
        DrawRig(MiddleRig);
        DrawRig(BottomRig);
        
        CalculateSpline();
        MoveCamera();
    }

    private void DrawRig(Rig rig)
    {
        var position = Target.Position;
        position.Y += rig.Height;
        
        DebugDraw.DrawCircle(position, Vector3.Up, rig.Radius, Color.Red);
    }

    private void CalculateCameraPosition()
    {
        CalculateCameraVerticalPosition();
        CalculateCameraOrbitPosition();
    }

    private void CalculateCameraVerticalPosition()
    {
        var normalizedRange = (-1.0f, 1.0f);
        var trueRange = (BottomRig.Height, TopRig.Height);

        var normalizedHeight = Mathf.Clamp(VerticalPosition, -1.0f, 1.0f);
        _trueVerticalCameraPosition = Map.MapRange(normalizedHeight, normalizedRange, trueRange);
    }

    private void CalculateCameraOrbitPosition()
    {
        var position = Target.Position;
        
        var orbitRadius = _spline.Eval(new[] { _trueVerticalCameraPosition });
        position.Z -= orbitRadius[0];

        _orbitPosition = position;
    }

    private void MoveCamera()
    {
        CalculateCameraPosition();
        var finalPosition = _orbitPosition;
        finalPosition.Y += _trueVerticalCameraPosition;

        Actor.Position = finalPosition;
    }

    private void CalculateSpline()
    {
        var xValues = new[] {BottomRig.Height, MiddleRig.Height, TopRig.Height};
        var yValues = new[] {BottomRig.Radius, MiddleRig.Radius, TopRig.Radius};

        _spline = new CubicSpline(xValues, yValues);
    }
}