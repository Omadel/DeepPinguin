using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

    public void Dig(int? dmg = null) {
        this.player.Dig(dmg != null ? null : dmg);
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
                    this.player.AddACDigDamage(stats.Amount);
                    break;
                case BonusTypes.AutoClickFrequency:
                    this.player.ImproveACFrequency();
                    break;
                case BonusTypes.SwimSpeed:
                    this.player.AddSwimSpeed(stats.Amount);
                    break;
                case BonusTypes.UnlockAutoClick:
                    this.ui.BuyAC.GetComponent<StoreElement>().Disable();
                    this.ui.Autoclick.SetActive(true);
                    print(this.ui.Autoclick.GetComponentInChildren<Button>().gameObject.name);
                    this.ui.Autoclick.GetComponentInChildren<Button>().onClick.AddListener(() => this.player.ToggleAutoClick());
                    foreach(GameObject parameter in this.ui.ACParameters) {
                        parameter.SetActive(true);
                    }
                    break;
                default:
                    break;
            };
            storeElement.AddPrice = storeElement.AddPrice * 2;

            this.ui.LevelText.text = storeElement.Level.ToString();


        }
    }

    public void SetPlayerState(int stateIndex) => this.player.ChangeState((PlayerState)stateIndex);
    public void SetPlayerState(string stateString) => this.player.ChangeState((PlayerState)System.Enum.Parse(typeof(PlayerState), stateString));

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
