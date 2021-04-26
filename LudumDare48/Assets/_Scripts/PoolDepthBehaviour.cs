using DG.Tweening;
using UnityEngine;

public class PoolDepthBehaviour : MonoBehaviour {

    public int Depth { get => this.poolDepth; }
    public Ease DigEase { get => this.digEase; }
    public float DigDuration { get => this.digDuration; }

    [SerializeField] [Min(0)] private int poolDepth = 1;
    [SerializeField] [Range(0, 20)] private int layerAmount = 1;
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
        GameManager.Instance.LayerBar.maxValue = 1;
    }

    public bool Dig(int damage) {
        this.layerHealth -= damage;
        GameManager.Instance.LayerBar.DOComplete();
        GameManager.Instance.LayerBar.transform.DOComplete();
        UnityEngine.UI.Image image = GameManager.Instance.LayerBar.GetComponentsInChildren<UnityEngine.UI.Image>()[1];
        image.DOComplete();
        Color color = image.color;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(this.layerHurtColor, .1f));
        sequence.Append(image.DOColor(color, .1f));
        GameManager.Instance.LayerBar.DOValue(this.layerHealth, .1f).Play();
        GameManager.Instance.LayerBar.transform.DOPunchScale(Vector3.one * .4f, .1f, 1, 0);
        GameManager.Instance.LayerBar.transform.DOShakePosition(.1f);
        GameManager.Instance.LayerBar.transform.DOShakeRotation(.1f, 4f);
        this.impulseSource.GenerateImpulse();
        if(this.layerHealth <= 0) {
            FindObjectOfType<AudioManager>().Play("Break");
            GameObject.Destroy(Instantiate(this.fxPrefab, new Vector3(GameManager.Instance.Player.transform.position.x, GameManager.Instance.Player.transform.position.y - 3, GameManager.Instance.Player.transform.position.z), Quaternion.identity), 1f);
            this.poolDepth++;
            this.depth.transform.DOScaleY(Mathf.Max(.001f, this.poolDepth), this.digDuration).SetEase(this.digEase);
            this.layers.transform.DOLocalMoveY(-this.poolDepth, this.digDuration).SetEase(this.digEase);
            this.layerHealth = this.poolDepth;
            GameManager.Instance.LayerBar.maxValue = this.poolDepth;
            GameManager.Instance.Score.text = this.poolDepth.ToString();
            GameManager.Instance.Player.transform.DOMoveY(-this.poolDepth, this.digDuration).SetEase(this.digEase);
            return true;
        }
        return false;
    }

    public Material GetMaterial(int index) {
        return this.layerMaterials[index];
    }

    private int layerHealth;
    private Cinemachine.CinemachineImpulseSource impulseSource;
}
