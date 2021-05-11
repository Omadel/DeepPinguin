using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandeler : MonoBehaviour {
    public Button ClickableArea { get => this.clickableArea; }
    public SwimBehaviour SwimBehaviour { get => this.swimBehaviour; }
    public TextMeshProUGUI ScoreText { get => this.scoreText; }
    public TextMeshProUGUI MoneyText { get => this.moneyText; }
    public TextMeshProUGUI DigDmgText { get => this.digDmgText; }
    public TextMeshProUGUI LevelText { get => this.levelText; }
    public TextMeshProUGUI EchelonsText { get => this.echelonsText; }
    public Slider BreathBar { get => this.breathBar; }
    public Slider LayerBar { get => this.layerBar; }
    public GameObject Stats { get => this.stats; }
    public GameObject Store { get => this.store; }
    public GameObject Digging { get => this.digging; }
    public GameObject Autoclick { get => this.autoclick; }
    public GameObject BuyAC { get => this.buyAC; }
    public GameObject[] ACParameters { get => this.aCParameters; }

    [Header("Gameplay")]
    [SerializeField] private Button clickableArea;
    [SerializeField] private SwimBehaviour swimBehaviour;
    [Header("TextFields")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI digDmgText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI echelonsText;
    [Header("Sliders")]
    [SerializeField] private Slider breathBar;
    [SerializeField] private Slider layerBar;
    [Header("Animatable Gameobjects")]
    [SerializeField] private GameObject stats;
    [SerializeField] private GameObject store;
    [SerializeField] private GameObject digging;
    [SerializeField] private GameObject autoclick;
    [Header("Store")]
    [SerializeField] private GameObject buyAC;
    [SerializeField] private GameObject[] aCParameters;

    private void Start() {
        this.layerBarImage = this.layerBar.GetComponentsInChildren<Image>()[1];
        InitializeTexts();
    }

    private void InitializeTexts() {
        GameManager gm = GameManager.Instance;
        this.scoreText.text = gm.Pool.Depth.ToString();
        this.moneyText.text = gm.Player.Money.ToString();
        this.digDmgText.text = gm.Player.DigDamage.ToString();
        //this.levelText.text;
        this.echelonsText.text = gm.Echelons.Current.ToString();
        TextMeshProUGUI[] texts = this.Autoclick.GetComponentsInChildren<TextMeshProUGUI>(true);
        texts[1].text = $"{gm.Player.AutoClickerFrequency * 1000f} ms";
        texts[2].text = gm.Player.AutoClickerDigDamage.ToString();
    }

    public void ChangeText(TextMeshProUGUI textMeshPro, string text, Color? color = null) {

        textMeshPro.text = text;
        textMeshPro.transform.DOComplete();
        textMeshPro.DOComplete();
        textMeshPro.transform.DOShakePosition(.3f, 3f);
        textMeshPro.transform.DOShakeRotation(.3f, 3f);
        textMeshPro.transform.DOPunchScale(Vector3.one * .2f, .3f);
        if(color != null) {
            textMeshPro.DOColor(color.Value, .1f).SetLoops(2, LoopType.Yoyo);
        }
    }


    public void PlayHurtLayerBarAnimation(int layerHealth, Color hurtColor) {
        this.layerBar.DOComplete();
        this.layerBar.transform.DOComplete();
        this.layerBarImage.DOComplete();

        this.layerBar.DOValue(layerHealth, .1f);
        this.layerBarImage.DOColor(hurtColor, .1f).SetLoops(2, LoopType.Yoyo);
        this.layerBar.transform.DOPunchScale(Vector3.one * .4f, .1f, 1, 0);
        this.layerBar.transform.DOShakePosition(.1f);
        this.layerBar.transform.DOShakeRotation(.1f, 4f);
    }

    public void UpdateLayerBar(int poolDepth) {
        this.layerBar.maxValue = poolDepth;
        this.layerBar.value = 0;
        this.layerBar.DOComplete();
        this.layerBar.DOValue(this.layerBar.maxValue, .2f);
        ChangeText(this.scoreText, poolDepth.ToString());
    }

    private Image layerBarImage;
}
