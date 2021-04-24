using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolDepthBehaviour))]
public class PoolDepthEditor : Editor {
    private SerializedProperty poolDepth, depth;

    private void Init() {
        this.poolDepth = this.serializedObject.FindProperty("poolDepth");
        this.depth = this.serializedObject.FindProperty("Depth");
    }
    private void OnValidate() {
        Init();
    }
    private void OnEnable() {
        Init();
    }
    private void OnSceneGUI() {
        GameObject depthGo = (GameObject)this.depth.objectReferenceValue;
        if(depthGo != null) {
            depthGo.transform.localScale = new Vector3(depthGo.transform.localScale.x, Mathf.Max(.001f, this.poolDepth.intValue), depthGo.transform.localScale.z);
        } else {
            Debug.LogError("Depth GameObject is not set");
        }
    }
}
