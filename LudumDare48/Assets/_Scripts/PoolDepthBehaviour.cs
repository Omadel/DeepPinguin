using DG.Tweening;
using UnityEngine;

public class PoolDepthBehaviour : MonoBehaviour {

    public int Depth { get => this.poolDepth; }
    public Ease DigEase { get => this.digEase; }
    public float DigDuration { get => this.digDuration; }

    [SerializeField] [Min(0)] private int poolDepth = 1;
    [SerializeField] private GameObject depth = null, layers = null;
    [SerializeField] private Material[] layerMaterials = new Material[20];
    [SerializeField] private Color layerHurtColor = Color.red;
    [SerializeField] private GameObject fxPrefab = null;


    [Header("Dig Animation Parameters")]
    [SerializeField] private float digDuration = .5f;
    [SerializeField] private Ease digEase = Ease.OutQuad;

    private void Start() {
        this.impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        this.layerHealth = this.poolDepth;
        GameManager.Instance.UI.LayerBar.maxValue = 1;
    }

    public bool Dig(int damage) {
        this.layerHealth -= damage;
        GameManager gm = GameManager.Instance;
        gm.UI.PlayHurtLayerBarAnimation(this.layerHealth, this.layerHurtColor);
        this.impulseSource.GenerateImpulse();
        if(this.layerHealth <= 0) {
            //todo correct audio manager
            FindObjectOfType<AudioManager>().Play("Break");
            //todo pooling VFX
            GameObject.Destroy(Instantiate(this.fxPrefab, new Vector3(gm.Player.transform.position.x, gm.Player.transform.position.y - 3, gm.Player.transform.position.z), Quaternion.identity), 2f);
            this.poolDepth++;
            this.depth.transform.DOScaleY(Mathf.Max(.001f, this.poolDepth), this.digDuration).SetEase(this.digEase);
            this.layers.transform.DOLocalMoveY(-this.poolDepth, this.digDuration).SetEase(this.digEase);
            gm.UI.UpdateLayerBar(this.poolDepth);
            gm.Player.transform.DOMoveY(-this.poolDepth, this.digDuration).SetEase(this.digEase);
            this.layerHealth = this.poolDepth;
            return true;
        }
        return false;
    }
#if UNITY_EDITOR
    public Material GetMaterial(int index) {
        return this.layerMaterials[index];
    }
#endif

    private int layerHealth;
    private Cinemachine.CinemachineImpulseSource impulseSource;
}
