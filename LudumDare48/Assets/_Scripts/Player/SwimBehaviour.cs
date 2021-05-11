using System.Collections;
using UnityEngine;

public class SwimBehaviour : MonoBehaviour {

    [SerializeField] private GameObject[] bonusPrefabs = null;
    [Tooltip("x=>Min y=>Max")]
    [SerializeField] private Vector2 rangeSpawnTime = new Vector2(1f, 3f), spawnRange = new Vector2(1, 7.5f);
    [SerializeField] private float distanceSpawnedFromPlayer = 10f;

    private void Awake() {
        this.player = GameManager.Instance.Player;
        this.sphereCollider = this.player.GetComponent<SphereCollider>();
    }

    private void OnEnable() {
        this.spawnCollectibles = StartCoroutine(SpawnCollectible(this.rangeSpawnTime.x, this.rangeSpawnTime.y));
        this.sphereCollider.enabled = true;
        this.rb = this.player.gameObject.AddComponent<Rigidbody>();
        this.rb.useGravity = false;
        this.rb.isKinematic = true;
    }

    private void OnDisable() {
        if(this.spawnCollectibles != null) {
            StopCoroutine(this.spawnCollectibles);
        }
        this.moveDir = Vector3.zero;
        this.sphereCollider.enabled = false;
        GameObject.Destroy(this.rb);
        this.rb = null;
    }

    private IEnumerator SpawnCollectible(float min, float max) {
        while(this.player.gameObject.transform.position.y > -GameManager.Instance.Pool.Depth + this.distanceSpawnedFromPlayer + 5) {
            yield return new WaitForSecondsRealtime(Random.Range(min, max));
            Vector3 offset = GetSpawnPos(this.spawnRange.x, this.spawnRange.y);
            Instantiate(this.bonusPrefabs[0], Vector3.up * this.player.transform.position.y + offset + Vector3.down * this.distanceSpawnedFromPlayer, Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPos(float min, float max) {
        float tmp = Random.Range(min, max);
        return new Vector3(tmp, 0, -(max + min - tmp));
    }

    private void Update() {
        Vector3 newPos = this.player.transform.position + this.moveDir * this.player.SideSwimSpeed * Time.deltaTime;
        if(newPos.x < 7f && newPos.x > .5f && newPos.z < -.5f && newPos.z > -7f) {
            this.player.transform.position = newPos;
        }
        this.player.transform.position += Vector3.down * this.player.SwimSpeed * Time.deltaTime;
        if(this.player.transform.position.y <= -GameManager.Instance.Pool.Depth) {
            this.player.ChangeState(PlayerState.Dig);
        }
    }

    public void SetDirection(string direction) {
        switch(direction) {
            case "":
                this.moveDir = Vector3.zero;
                break;
            case "Left":
                this.moveDir = new Vector3(-1, 0, -1).normalized;
                break;
            case "Right":
                this.moveDir = new Vector3(1, 0, 1).normalized;
                break;
            default:
                break;
        }
    }

    private Coroutine spawnCollectibles;
    private Vector3 moveDir = Vector3.zero;
    private PlayerBehaviour player = null;
    private Rigidbody rb;
    private SphereCollider sphereCollider;
}
