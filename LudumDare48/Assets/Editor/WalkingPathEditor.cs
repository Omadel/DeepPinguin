using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WalkingPath))]
public class WalkingPathEditor : Editor {
    private Vector3[] waypoints = null;
    private void Init() {
        WalkingPath walkingPath = (WalkingPath)this.target;
        this.waypoints = walkingPath.Waypoints;
        this.waypoints = new Vector3[walkingPath.gameObject.transform.childCount];
        for(int i = 0; i < walkingPath.gameObject.transform.childCount; i++) {
            this.waypoints[i] = walkingPath.gameObject.transform.GetChild(i).position;
        }
        walkingPath.Waypoints = this.waypoints;

    }
    private void OnValidate() => Init();
    private void OnEnable() => Init();

    private void OnSceneGUI() {
        Handles.DrawAAPolyLine(this.waypoints);
    }

}
