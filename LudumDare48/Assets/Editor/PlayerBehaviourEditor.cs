using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerBehaviour))]
public class PlayerBehaviourEditor : Editor {
    private PlayerBehaviour player = null;
    private void OnSceneGUI() {
        this.player = this.target as PlayerBehaviour;
        Handles.DrawAAPolyLine(new Vector3[2] {
            this.player.transform.position,this.player.transform.position+this.player.transform.forward
        });
    }
}
