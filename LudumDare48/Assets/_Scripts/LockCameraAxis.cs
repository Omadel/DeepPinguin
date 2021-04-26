using Cinemachine;
using UnityEngine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's chosen Axis co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraAxis : CinemachineExtension {
    [SerializeField] private LockAxis[] lockAxes = new LockAxis[] { new LockAxis() };

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
        if(stage == CinemachineCore.Stage.Body) {
            Vector3 pos = state.RawPosition;
            for(int i = 0; i < this.lockAxes.Length; i++) {

                switch(this.lockAxes[i].Axis) {
                    case Axis.X:
                        pos.x = this.lockAxes[i].LockPosition;
                        break;
                    case Axis.Y:
                        pos.y = this.lockAxes[i].LockPosition;
                        break;
                    case Axis.Z:
                        pos.z = this.lockAxes[i].LockPosition;
                        break;
                    default:
                        break;
                }
            }
            state.RawPosition = pos;
        }
    }
}
public enum Axis { X, Y, Z };

[System.Serializable]
public struct LockAxis {
    public Axis Axis { get => this.axis; }
    public float LockPosition { get => this.lockPosition; }

    [SerializeField] private Axis axis;
    [SerializeField] private float lockPosition;


    public LockAxis(Axis axis = Axis.X, float lockPosition = 10f) {
        this.axis = axis;
        this.lockPosition = lockPosition;
    }
}