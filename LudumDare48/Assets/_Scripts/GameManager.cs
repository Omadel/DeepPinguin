using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public PoolDepthBehaviour Pool { get => this.pool; }
    public PlayerBehaviour Player { get => this.player; }
    public EchelonBehaviour Echelons { get => this.echelons; }
    public GameObject WaterPoolGo { get => this.waterPoolGo; }
    public int MoneyMultiplicator { get => this.moneyMultiplicator; }
    public UIHandeler UI { get => this.ui; }

    [SerializeField] private PoolDepthBehaviour pool = null;
    [SerializeField] private PlayerBehaviour player = null;
    [SerializeField] private EchelonBehaviour echelons = null;
    [SerializeField] private GameObject layerGo = null, waterPoolGo = null;
    [SerializeField] private int moneyMultiplicator = 1;
    [SerializeField] private UIHandeler ui;




    private void Awake() {
        if(GameManager.Instance == null) {
            GameManager.Instance = this;
        } else {
            GameObject.Destroy(this.gameObject);
            return;
        }
        Application.targetFrameRate = 120;
        //QualitySettings.vSyncCount = 60;

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

    public void BuyAutoClick(GameObject button) {
        if(this.player.Money >= 25) {
            button.SetActive(false);
            this.ui.Autoclick.SetActive(true);
        }
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
                    this.moneyMultiplicator += stats.Amount;
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

            this.ui.LevelText.text = storeElement.Level.ToString();


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
