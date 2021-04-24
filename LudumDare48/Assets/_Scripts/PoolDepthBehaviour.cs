using UnityEngine;

public class PoolDepthBehaviour : MonoBehaviour {

    [SerializeField] [Min(0)] private int poolDepth = 1;
    [SerializeField] [Range(0, 20)] private int layerAmount = 1;
    [SerializeField] private GameObject depth, layers;
    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        this.depth.transform.localScale = new Vector3(this.depth.transform.localScale.x, Mathf.Max(.001f, this.poolDepth), this.depth.transform.localScale.z);
        this.layers.transform.position += Vector3.down * this.poolDepth;
    }


}
