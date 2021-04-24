using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolDepthBehaviour))]
public class PoolDepthEditor : Editor {
    private SerializedProperty poolDepth, depth, layers, layerAmount;

    private void Init() {
        this.poolDepth = this.serializedObject.FindProperty("poolDepth");
        this.layerAmount = this.serializedObject.FindProperty("layerAmount");

        this.depth = this.serializedObject.FindProperty("depth");
        this.layers = this.serializedObject.FindProperty("layers");
    }
    private void OnValidate() {
        Init();
    }
    private void OnEnable() {
        Init();
    }
    private void OnSceneGUI() {
        HandlePoolDepth();
        HandleLayerAmount();
    }

    private void HandleLayerAmount() {
        GameObject layersGo = (GameObject)this.layers.objectReferenceValue;
        int currentLayerAmount = layersGo.transform.childCount;
        if(this.layerAmount.intValue != currentLayerAmount) {
            GameObject layerPrefab = Resources.Load("Prefabs/Layer") as GameObject;
            if(this.layerAmount.intValue > currentLayerAmount) {
                GameObject.Instantiate(layerPrefab, layersGo.transform, false);
            } else {

            }
        }
    }

    private void HandlePoolDepth() {
        GameObject depthGo = (GameObject)this.depth.objectReferenceValue;
        if(depthGo != null) {
            depthGo.transform.localScale = new Vector3(depthGo.transform.localScale.x, Mathf.Max(.001f, this.poolDepth.intValue), depthGo.transform.localScale.z);
            GameObject layersGo = (GameObject)this.layers.objectReferenceValue;
            if(layersGo != null) {

                layersGo.transform.position = new Vector3(layersGo.transform.position.x, -this.poolDepth.intValue, layersGo.transform.position.z);
            } else {
                Debug.LogError("Layers GameObject is not set");
            }
        } else {
            Debug.LogError("Depth GameObject is not set");
        }
    }
}
