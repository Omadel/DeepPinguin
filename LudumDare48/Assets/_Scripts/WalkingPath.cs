using UnityEngine;

public class WalkingPath : MonoBehaviour {
    public Vector3[] Waypoints { get => this.waypoints; set => this.waypoints = value; }
    [SerializeField] private Vector3[] waypoints = null;


    private void Start() {
        this.waypoints = new Vector3[this.transform.childCount];
        for(int i = 0; i < this.transform.childCount; i++) {
            this.waypoints[i] = this.transform.GetChild(i).position;
        }
    }

    private void Update() {

    }
}
