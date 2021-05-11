using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolDepthBehaviour))]
public class PoolDepthEditor : Etienne.Editor<PoolDepthBehaviour> {
    private SerializedProperty poolDepth, depth, layers;
    private int layerAmount;


    private void Init() {
        poolDepth = serializedObject.FindProperty("poolDepth");

        depth = serializedObject.FindProperty("depth");
        layers = serializedObject.FindProperty("layers");
    }
    private void OnValidate() {
        Init();
    }
    private void OnEnable() {
        Init();
    }

    public override void OnInspectorGUI() {
        // this.layerAmount = (int)GUILayout.HorizontalSlider(this.layerAmount, 0, 20);
        if(GUILayout.Button("Fix Depth")) {
            HandlePoolDepth();
        }
        if(GUILayout.Button("Spawn Layers")) {
            HandleLayerAmount();
        }
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Layer Amount");
        layerAmount = EditorGUILayout.IntSlider(layerAmount, 0, 20);
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    private void OnSceneGUI() {
        //HandlePoolDepth();
        //HandleLayerAmount();
    }

    private void HandleLayerAmount() {
        GameObject layersGo = layers.objectReferenceValue as GameObject;
        int currentLayerAmount = layersGo.transform.childCount;
        Debug.Log($"child count : {currentLayerAmount} target layer amount : {layerAmount}");
        if(layerAmount != currentLayerAmount) {
            GameObject layerPrefab = Resources.Load("Prefabs/Gameplay/Layer") as GameObject;
            while(layerAmount > currentLayerAmount) {
                GameObject layer = GameObject.Instantiate(layerPrefab, layersGo.transform, false);
                layer.transform.localPosition = new Vector3(0, -currentLayerAmount, 0);
                foreach(Transform quad in layer.transform) {
                    quad.GetComponent<MeshRenderer>().material = Target.GetMaterial(currentLayerAmount);
                }
                currentLayerAmount++;
            }
            while(layerAmount < currentLayerAmount) {
                GameObject.DestroyImmediate(layersGo.transform.GetChild(currentLayerAmount - 1).gameObject);
                currentLayerAmount--;
            }
        }
    }

    private void HandlePoolDepth() {
        GameObject depthGo = (GameObject)depth.objectReferenceValue;
        if(depthGo != null) {
            depthGo.transform.Find("WaterTop").gameObject.SetActive(poolDepth.intValue > 0);
            depthGo.transform.localScale = new Vector3(depthGo.transform.localScale.x, Mathf.Max(.001f, poolDepth.intValue), depthGo.transform.localScale.z);
            GameObject layersGo = (GameObject)layers.objectReferenceValue;
            if(layersGo != null) {

                layersGo.transform.position = new Vector3(layersGo.transform.position.x, -poolDepth.intValue, layersGo.transform.position.z);
            } else {
                Debug.LogError("Layers GameObject is not set");
            }
        } else {
            Debug.LogError("Depth GameObject is not set");
        }
    }
}
