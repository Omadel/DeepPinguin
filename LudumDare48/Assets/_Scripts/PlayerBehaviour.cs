using DG.Tweening;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] private int digDamage = 1;
    private void Start() {

    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            PoolDepthBehaviour pool = GameManager.Instance.Pool;
            pool.Dig(this.digDamage);
            this.transform.DOMoveY(-pool.Depth, pool.DigDuration).SetEase(pool.DigEase);
        }
    }
}
