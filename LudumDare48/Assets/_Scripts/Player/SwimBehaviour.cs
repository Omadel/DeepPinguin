using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimBehaviour : MonoBehaviour {

    [SerializeField] private GameObject[] bonusPrefabs = null;
    [Tooltip("x=>Min y=>Max")]
    [SerializeField] private Vector2 rangeSpawnTime = new Vector2(1f, 3f);

    private void OnEnable() {
        this.player = GameManager.Instance.Player;
        this.spawnCollectibles = StartCoroutine(SpawnCollectible(this.rangeSpawnTime.x, this.rangeSpawnTime.y));
    }

    private void OnDisable() {
        StopCoroutine(this.spawnCollectibles);
        this.moveDir = Vector3.zero;
    }

    private IEnumerator SpawnCollectible(float min, float max) {
        while(true) {
            yield return new WaitForSecondsRealtime(Random.Range(min, max));
            this.collectibles.Add(Instantiate(this.bonusPrefabs[0], this.player.transform.position + Vector3.down * 10, Quaternion.identity));
            Debug.Log("Spawn <color=blue>Fish</color>");
        }
    }

    private void Update() {
        Vector3 newPos = this.player.transform.position + this.moveDir * this.player.SideSwimSpeed * Time.deltaTime;
        if(newPos.x < 7f && newPos.x > .5f && newPos.z < -.5f && newPos.z > -7f) {
            this.player.transform.position = newPos;
        }
        this.player.transform.position += Vector3.down * this.player.SwimSpeed * Time.deltaTime;
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
        print(direction);
    }

    private List<GameObject> collectibles = new List<GameObject>();
    private Coroutine spawnCollectibles;
    private Vector3 moveDir = Vector3.zero;
    private PlayerBehaviour player = null;
}
