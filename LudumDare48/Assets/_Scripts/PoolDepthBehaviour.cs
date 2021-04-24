using DG.Tweening;
using UnityEngine;

public class PoolDepthBehaviour : MonoBehaviour {

    public int Depth { get => this.poolDepth; }
    public Ease DigEase { get => this.digEase; }
    public float DigDuration { get => this.digDuration; }

    [SerializeField] [Min(0)] private int poolDepth = 1;
    [SerializeField] [Range(0, 20)] private int layerAmount = 1;
    [SerializeField] private GameObject depth = null, layers = null;

    [Header("Dig Animation Parameters")]
    [SerializeField] private float digDuration = .5f;
    [SerializeField] private Ease digEase = Ease.OutQuad;

    private void Start() {
        this.layerHealth = this.poolDepth;
    }

    public void Dig(int damage) {
        this.layerHealth -= damage;
        if(this.layerHealth <= 0) {
            this.poolDepth++;
            this.depth.transform.DOScaleY(Mathf.Max(.001f, this.poolDepth), this.digDuration).SetEase(this.digEase);
            this.layers.transform.DOLocalMoveY(-this.poolDepth, this.digDuration).SetEase(this.digEase);
            this.layerHealth = this.poolDepth;
        }
        //this.transform.DOComplete();
        //this.transform.DOPunchScale(this.transform.position + Vector3.one * .05f, .5f);
    }

    private int layerHealth;
}
