using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PoolDepthBehaviour Pool { get => this.pool; }
    public TextMeshProUGUI Score { get => this.scoreText; }
    public TextMeshProUGUI Money { get => this.moneyText; }
    public TextMeshProUGUI Level { get => this.moneyText; }

    public TextMeshProUGUI DigDamage { get => this.digDmgText; }
    public Slider BreathBar { get => this.breathBar; }
    public Slider LayerBar { get => this.layerBar; }
    public PlayerBehaviour Player { get => this.player; }
    public PalierApparition Palier { get => this.palier; }
    public GameObject Stats { get => this.stats; }
    public GameObject Store { get => this.store; }
    public GameObject Digging { get => this.digging; }
    public Button ClickableArea { get => this.clickableArea; }
    public GameObject WaterPoolGo { get => this.waterPoolGo; }
    public SwimBehaviour SwimBehaviour { get => this.swimBehaviour; }
    public int AddMoney { get => this.addMoney; }



    [SerializeField] private PoolDepthBehaviour pool = null;
    [SerializeField] private PlayerBehaviour player = null;
    [SerializeField] private PalierApparition palier = null;
    [SerializeField] private GameObject layerGo = null, waterPoolGo = null;
    [SerializeField] private int addMoney=1;



    [Header("UI")]
    [SerializeField] private Button clickableArea = null;
    [SerializeField] private SwimBehaviour swimBehaviour = null;
    [Header("TextFields"), Space(-10)]
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI moneyText = null, digDmgText = null;
    [SerializeField] private TextMeshProUGUI LevelText = null;
    [SerializeField] private Slider breathBar = null, layerBar = null;
    [SerializeField] private GameObject stats = null, store = null, digging = null;




    private void Awake() {
        if(GameManager.Instance == null) {
            GameManager.Instance = this;
        } else {
            GameObject.Destroy(this.gameObject);
        }
        Application.targetFrameRate = 60;

#if UNITY_EDITOR
        if(this.player == null) {
            Debug.LogError("Player is not specified");
            Application.Quit();
        }
#endif
    }

    public void Dig() {
        this.player.Dig();
        this.layerGo.transform.eulerAngles += Vector3.up * 90;
    }

    public void Buy(StoreElementStats stats, StoreElement storeElement) {
        if(this.player.Money >= stats.Cost * storeElement.AddPrice) {
            this.player.AddMoney(-stats.Cost * storeElement.AddPrice);
            switch(stats.BonusTypes) {
                case BonusTypes.BreathTime:
                    this.player.AddBreathTime(stats.Amount);
                    storeElement.Level += 1;
                    break;
                case BonusTypes.Money:
                    addMoney = addMoney * stats.Amount;
                    //this.player.AddMoney(stats.Amount);
                    break;
                case BonusTypes.DigStrenght:
                    this.player.AddDigDamage(stats.Amount);
                    break;
                case BonusTypes.AutoClicksDamage:
                    this.player.SetAutoClicker(false, stats.Amount);
                    break;
                case BonusTypes.AutoClickFrequency:
                    this.player.SetAutoClicker(false, 0, true);
                    break;
                case BonusTypes.SwimSpeed:
                    this.player.AddSwimSpeed(stats.Amount);
                    break;
                default:
                    break;
            };
            storeElement.AddPrice = storeElement.AddPrice * 2;

            this.LevelText.text = storeElement.Level.ToString();


        }
    }

    public void SetPlayerState(int stateIndex) => this.player.SetState((PlayerState)stateIndex);
    public void SetPlayerState(string stateString) => this.player.SetState((PlayerState)System.Enum.Parse(typeof(PlayerState), stateString));

    public void UIShow(GameObject UIGameObject) => UIGameObject.SetActive(true);
    public void UIHide(GameObject UIGameObject) => UIGameObject.SetActive(false);

    public void UIPanUP(GameObject UIGameObject) {
        UIShow(UIGameObject);
        Vector3 position = UIGameObject.transform.position;
        UIGameObject.transform.position = new Vector3(position.x, -10, position.z);
        UIGameObject.transform.DOMoveY(position.y, .4f);
    }

    public void UIPanDown(GameObject UIGameObject) {
        Vector3 position = UIGameObject.transform.position;
        UIGameObject.transform.DOMoveY(-10, .2f).OnComplete(() => {
            UIGameObject.transform.position = position;
            UIHide(UIGameObject);
        });
    }
}
