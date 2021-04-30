using UnityEngine;

public class DecorationHandeler : MonoBehaviour {
    [SerializeField] private GameObject[] decorations;

    private void Update() {
        if(this.transform.position.y < 0) {
            if(this.transform.position.y <= -36 * (2 + (this.time))) {
                this.decorations[this.time % this.decorations.Length].transform.position
                    += Vector3.down * 36 * this.decorations.Length;
                this.time++;
            }
            if(this.time != 0 && this.transform.position.y > -36 * (1 + this.time)) {
                this.decorations[(this.time - 1) % this.decorations.Length].transform.position
                    += Vector3.up * 36 * this.decorations.Length;
                this.time--;
            }
        }
    }

    private int time;
}
