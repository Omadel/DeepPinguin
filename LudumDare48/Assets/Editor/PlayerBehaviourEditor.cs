using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerBehaviour))]
public class PlayerBehaviourEditor : Etienne.Editor<PlayerBehaviour> {
    private void OnSceneGUI() {
        Handles.DrawAAPolyLine(new Vector3[2] {
            Target.transform.position,Target.transform.position+Target.transform.forward
        });
    }
}
