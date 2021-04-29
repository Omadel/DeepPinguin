using UnityEngine;
public class DestroyParticles : MonoBehaviour {
    private void Start() {
        Destroy(this.gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}