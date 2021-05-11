using UnityEditor;
using UnityEngine;

namespace Etienne {
    [CustomEditor(typeof(ColliderActions))]
    public class ColliderActionsEditor : Editor<ColliderActions> {

        private void OnEnable() {
            onTriggerEnter = serializedObject.FindProperty("onTriggerEnter");
            onTriggerExit = serializedObject.FindProperty("onTriggerExit");
            onTriggerStay = serializedObject.FindProperty("onTriggerStay");
            onCollisionEnter = serializedObject.FindProperty("onCollisionEnter");
            onCollisionExit = serializedObject.FindProperty("onCollisionExit");
            onCollisionStay = serializedObject.FindProperty("onCollisionStay");
            needGO = serializedObject.FindProperty("needGO");
            ignoreGameObject = serializedObject.FindProperty("ignoreGameObject");
            ignoredGO = serializedObject.FindProperty("ignoredGO");

        }
        public override void OnInspectorGUI() {
            ShowNeedGO();
            ShowIgnoreGO();

            CheckIsTrigger();

            EditorGUI.BeginChangeCheck();
            if(isTrigger) {
                EditorGUILayout.PropertyField(onTriggerEnter);
                EditorGUILayout.PropertyField(onTriggerExit);
                EditorGUILayout.PropertyField(onTriggerStay);
            } else {
                EditorGUILayout.PropertyField(onCollisionEnter);
                EditorGUILayout.PropertyField(onCollisionExit);
                EditorGUILayout.PropertyField(onCollisionStay);
            }
            if(EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }


        private void CheckIsTrigger() {
            isTrigger = Target.gameObject.GetComponent<Collider>().isTrigger;
        }
        private void ShowIgnoreGO() {
            if(GUILayout.Button(ignoreGameObject.boolValue ? "Ignore Certain GameObject(s)" : "No GameObject Is Ignored")) {
                ignoreGameObject.boolValue = !ignoreGameObject.boolValue;
            }
            if(ignoreGameObject.boolValue) {
                EditorGUILayout.PropertyField(ignoredGO);
            }
        }

        private void ShowNeedGO() {
            if(GUILayout.Button(needGO.boolValue ? "GameObject Specific" : "All GameObject Can Interact")) {
                needGO.boolValue = !needGO.boolValue;
            }
            if(needGO.boolValue) {
                GUILayout.BeginHorizontal();
                GUILayout.Label("GameObject");
                Target.Go = (GameObject)EditorGUILayout.ObjectField(Target.Go, typeof(GameObject), true);
                if(Target.Go != null) {
                    CheckCollider();
                    if(GUILayout.Button("X", GUILayout.MaxWidth(20f))) {
                        Target.Go = null;
                    }
                }
                GUILayout.EndHorizontal();
                System.Text.StringBuilder msg = new System.Text.StringBuilder("No GameObject Selected");
                GUIStyle style = new GUIStyle() {
                    fontSize = 15,
                    fontStyle = FontStyle.Bold
                };
                style.normal.textColor = new Color(237 / 255f, 44 / 255f, 63 / 255f);
                if(Target.Go == null) {
                    if(Application.isPlaying) {
                        EditorApplication.ExitPlaymode();
                        Debug.LogError($"{msg} from : {Target.gameObject.name}");
                    }
                } else {

                    if(!hasRb && !hasCC) {
                        msg.Clear();
                        msg.Append("No Rigidbody detected in the selected GameObject.");
                        if(Application.isPlaying) {
                            EditorApplication.ExitPlaymode();
                            Debug.LogError($"{msg} from : {Target.gameObject.name}");
                        }
                    } else {
                        msg.Clear();
                        style.normal.textColor = new Color(51 / 255f, 204 / 255f, 51 / 255f);
                        if(hasCC) {
                            msg.Append("Character Controller");
                        } else {
                            msg.Append("Rigidbody");
                        }
                        msg.Append(" Found !");
                    }
                }
                GUILayout.Label(msg.ToString(), style);
            }
        }
        private void CheckCollider() {

            hasRb = Target.Go.TryGetComponent(out Rigidbody _);
            hasCC = Target.Go.TryGetComponent(out CharacterController _);
        }

        private SerializedProperty onTriggerEnter, onTriggerExit, onTriggerStay;
        private SerializedProperty onCollisionEnter, onCollisionExit, onCollisionStay;
        private SerializedProperty needGO, ignoreGameObject, ignoredGO;

        private bool isTrigger, hasCC, hasRb;
    }
}