using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public int DigDamage { get => this.digDamage; set => this.digDamage = value; }
    public int Money { get => this.money; set => this.money = value; }
    public int BreathTime { get => this.breathTime; set => this.breathTime = value; }
    public float SwimSpeed { get => this.swimSpeed; }
    public float SideSwimSpeed { get => this.sideSwimSpeed; }

    [SerializeField] private int digDamage = 1;
    [SerializeField] [Min(0)] private int money = 0;
    [SerializeField] private WalkingPath walkingPath = null;
    [Header("Walk")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private Ease walkingEase = Ease.OutQuad;

    [Header("Swim")]
    [SerializeField] private int breathTime = 20;
    [SerializeField] private float swimSpeed = 6f, sideSwimSpeed = 8f;
    private void Start() {
        this.gameManager = GameManager.Instance;
        this.swimBehaviour = this.gameManager.SwimBehaviour;
    }

    private IEnumerator TimerBreath(float breathTime = 20f) {
        float timeLeft = breathTime;
        this.gameManager.BreathBar.maxValue = timeLeft;
        this.gameManager.BreathBar.value = timeLeft;
        while(timeLeft >= 0) {
            yield return new WaitForSecondsRealtime(1);
            this.gameManager.BreathBar.value = timeLeft;
            timeLeft--;
        }
        SetState(PlayerState.GainMoney);
    }

    private void GainMoney() {
        StartCoroutine(SetScore(this.gameManager.Pool.Depth));
        Sequence sequence = DOTween.Sequence();
        for(int i = 1; i < this.walkingPath.Waypoints.Length; i++) {
            float duration = Vector3.Distance(this.transform.position, this.walkingPath.Waypoints[i]) / this.walkSpeed;
            sequence.Append(this.transform.DOMove(this.walkingPath.Waypoints[i], duration).SetEase(this.walkingEase));
        }
    }

    private void Update() {
        if(this.state == PlayerState.Swim && this.transform.position.y <= -this.gameManager.Pool.Depth) {
            SetState(PlayerState.Dig);
        }
        if(this.state == PlayerState.Idle) {

        }
    }

    public void Dig() {
        PoolDepthBehaviour pool = this.gameManager.Pool;
        if(pool.Dig(this.digDamage)) {
            FindObjectOfType<AudioManager>().Play("Dig");
            this.gameManager.WaterPoolGo.SetActive(true);
            this.dugLayers++;
        }
    }

    private IEnumerator SetScore(int amount) {
        while(this.dugLayers > 0) {
            FindObjectOfType<AudioManager>().Play("calcul");
            yield return new WaitForSecondsRealtime(.1f);
            this.dugLayers--;
            AddMoney(1);
        }
        FindObjectOfType<AudioManager>().Stop("calcul");
        SetState(PlayerState.Buy);
    }

    public void AddBreathTime(int amount) {
        this.breathTime += amount;
    }

    public void AddMoney(int amount) {
        this.money += amount;
        this.gameManager.Money.text = this.money.ToString();
    }

    public void AddDigDamage(int amount) {
        this.digDamage += amount;
        this.gameManager.DigDamage.text = this.digDamage.ToString();
    }

    public void SetState(PlayerState state) {
        print($"Changed state from {this.state} to {state}");
        switch(this.state) {
            case PlayerState.Idle:
                break;
            case PlayerState.Swim:
                this.swimBehaviour.gameObject.SetActive(false);
                break;
            case PlayerState.Dig:
                this.gameManager.UIPanDown(this.gameManager.Digging);
                this.gameManager.ClickableArea.onClick.RemoveAllListeners();
                this.gameManager.ClickableArea.interactable = false;
                break;
            case PlayerState.GainMoney:
                break;
            case PlayerState.Buy:
                this.gameManager.UIPanDown(this.gameManager.Store);
                break;
            default:
                break;
        }
        this.state = state;
        switch(state) {
            case PlayerState.Idle:
                break;
            case PlayerState.Swim:
                this.swimBehaviour.gameObject.SetActive(true);
                break;
            case PlayerState.Dig:
                this.gameManager.UIPanUP(this.gameManager.Digging);
                this.gameManager.ClickableArea.onClick.AddListener(() => Dig());
                this.gameManager.ClickableArea.interactable = true;
                StartCoroutine(TimerBreath(this.breathTime));
                break;
            case PlayerState.GainMoney:
                this.transform.DOMove(this.walkingPath.Waypoints[0], 2f).OnComplete(GainMoney);
                break;
            case PlayerState.Buy:
                this.gameManager.UIPanUP(this.gameManager.Store);
                break;
            default:
                break;
        }
    }


    private PlayerState state = PlayerState.Idle;
    private SwimBehaviour swimBehaviour = null;
    private GameManager gameManager = null;
    private int dugLayers = 0;

}
public enum PlayerState { Idle, Swim, Dig, GainMoney, Buy }
