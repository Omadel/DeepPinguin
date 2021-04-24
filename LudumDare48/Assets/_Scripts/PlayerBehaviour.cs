using DG.Tweening;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] private int digDamage = 1;
    [SerializeField] [Min(0)] private int money = 0;
    private void Start() {

    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
            PoolDepthBehaviour pool = GameManager.Instance.Pool;
            pool.Dig(this.digDamage);
            this.transform.DOMoveY(-pool.Depth, pool.DigDuration).SetEase(pool.DigEase);
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 3) {
            this.transform.DOMoveY(0, 2f);
        }
    }
}
