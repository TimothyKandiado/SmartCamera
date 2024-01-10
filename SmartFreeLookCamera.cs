using FlaxEngine;

namespace Tmore.SmartCamera;

/// <summary>
/// SmartFreeLookCamera Script.
/// Attached to an empty actor representing a virtual camera
/// </summary>
public class SmartFreeLookCamera : SmartVirtualCamera
{
    [Header("Settings")]
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    [Tooltip("Whether the camera should be capable of flying through the scene using Arrow keys")]
    public bool EnableMovement { get; set; } = true;

    private float pitch;
    private float yaw;
    
    
    public override void OnStart()
    {
        var initialEulerAngles = Actor.Orientation.EulerAngles;
        pitch = initialEulerAngles.X;
        yaw = initialEulerAngles.Y;
    }
    
    internal override void UpdateVirtualCamera()
    {
        Screen.CursorVisible = false;
        Screen.CursorLock = CursorLockMode.Locked;

        var mouseDelta = new Float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
        yaw += mouseDelta.X;
    }
    
    internal override void FixedUpdateVirtualCamera()
    {
        var camTrans = Actor.Transform;
        var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

        camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);

        if (EnableMovement)
        {
            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");
            var move = new Vector3(inputH, 0.0f, inputV);
            move.Normalize();
            move = camTrans.TransformDirection(move);

            camTrans.Translation += move * MoveSpeed;
        }

        Actor.Transform = camTrans;
    }
}