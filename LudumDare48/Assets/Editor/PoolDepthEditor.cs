using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolDepthBehaviour))]
public class PoolDepthEditor : Editor {
    private SerializedProperty poolDepth, depth, layers;
    private int layerAmount;


    private void Init() {
        this.poolDepth = this.serializedObject.FindProperty("poolDepth");

        this.depth = this.serializedObject.FindProperty("depth");
        this.layers = this.serializedObject.FindProperty("layers");
    }
    private void OnValidate() {
        Init();
    }
    private void OnEnable() {
        Init();
    }

    public override void OnInspectorGUI() {
        // this.layerAmount = (int)GUILayout.HorizontalSlider(this.layerAmount, 0, 20);
        if(GUILayout.Button("Spawn Layers")) {
            HandleLayerAmount();
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Layer Amount");
        this.layerAmount = EditorGUILayout.IntSlider(this.layerAmount, 0, 20);
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    private void OnSceneGUI() {
        HandlePoolDepth();
        //HandleLayerAmount();
    }

    private void HandleLayerAmount() {
        GameObject layersGo = (GameObject)this.layers.objectReferenceValue;
        int currentLayerAmount = layersGo.transform.childCount;
        Debug.Log($"child count : {currentLayerAmount} target layer amount : {this.layerAmount}");
        if(this.layerAmount != currentLayerAmount) {
            GameObject layerPrefab = Resources.Load("Prefabs/Gameplay/Layer") as GameObject;
            PoolDepthBehaviour pool = (PoolDepthBehaviour)this.target;
            while(this.layerAmount > currentLayerAmount) {
                GameObject layer = GameObject.Instantiate(layerPrefab, layersGo.transform, false);
                layer.transform.localPosition = new Vector3(0, -currentLayerAmount, 0);
                foreach(Transform quad in layer.transform) {
                    quad.GetComponent<MeshRenderer>().material = pool.GetMaterial(currentLayerAmount);
                }
                currentLayerAmount++;
            }
            while(this.layerAmount < currentLayerAmount) {
                GameObject.DestroyImmediate(layersGo.transform.GetChild(currentLayerAmount - 1).gameObject);
                currentLayerAmount--;
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
