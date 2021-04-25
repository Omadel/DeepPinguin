using UnityEngine;

public class WalkingPath : MonoBehaviour {
    public Vector3[] Waypoints { get => this.waypoints; set => this.waypoints = value; }
    [SerializeField] private Vector3[] waypoints = null;

}
