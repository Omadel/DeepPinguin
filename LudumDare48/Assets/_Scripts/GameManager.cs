using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PoolDepthBehaviour Pool { get => this.pool; }
    public TextMeshProUGUI Score { get => this.scoreText; }
    public TextMeshProUGUI Money { get => this.moneyText; }
    public TextMeshProUGUI DigDamage { get => this.digDmgText; }
    public Slider BreathBar { get => this.breathBar; }
    public PlayerBehaviour Player { get => this.player; }
    public GameObject Stats { get => this.stats; }
    public GameObject Store { get => this.store; }
    public GameObject Digging { get => this.digging; }
    public Button ClickableArea { get => this.clickableArea; }
    [SerializeField] private PoolDepthBehaviour pool = null;
    [SerializeField] private PlayerBehaviour player = null;

    [Header("UI")]
    [SerializeField] private Button clickableArea = null;
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI moneyText = null;
    [SerializeField] private TextMeshProUGUI digDmgText = null;
    [SerializeField] private Slider breathBar = null;
    [SerializeField] private GameObject stats = null;
    [SerializeField] private GameObject store = null;
    [SerializeField] private GameObject digging = null;

    private void Awake() {
        if(GameManager.Instance == null) {
            GameManager.Instance = this;
        } else {
            GameObject.Destroy(this.gameObject);
        }

        if(this.player == null) {
            Debug.LogError("Player is not specified");
            Application.Quit();
        }
    }

    public void Dig() => this.player.Dig();

    public void Buy(StoreElementStats stats) {
        if(this.player.Money >= stats.Cost) {
            this.player.AddMoney(-stats.Cost);
            switch(stats.BonusTypes) {
                case BonusTypes.BreathTime:
                    this.player.AddBreathTime(stats.Amount);
                    break;
                case BonusTypes.Money:
                    this.player.AddMoney(stats.Amount);
                    break;
                case BonusTypes.DigStrenght:
                    this.player.AddDigDamage(stats.Amount);
                    break;
                case BonusTypes.AutoClicksAmount:
                    break;
                default:
                    break;
            };
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
        UIGameObject.transform.DOMoveY(position.y, .2f);
    }

    public void UIPanDown(GameObject UIGameObject) {
        Vector3 position = UIGameObject.transform.position;
        UIGameObject.transform.DOMoveY(-10, .2f).OnComplete(() => {
            UIGameObject.transform.position = position;
            UIHide(UIGameObject);
        });
    }
}
