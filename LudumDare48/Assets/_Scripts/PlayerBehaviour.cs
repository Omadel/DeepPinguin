using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    public int DigDamage { get => this.digDamage; set => this.digDamage = value; }
    public int Money { get => this.money; set => this.money = value; }
    public int BreathTime { get => this.breathTime; set => this.breathTime = value; }
    public float SwimSpeed { get => this.swimSpeed; }
    public float SideSwimSpeed { get => this.sideSwimSpeed; }
    public PlayerState State { get => this.state; }

    private bool isAutoClicker = false;
    [Header("Auto clicker")]
    [SerializeField] private int autoClickerDigDamage = 0;
    [SerializeField] private float autoClickerFrequency = 3f;

    [Header("Player")]
    [SerializeField] private int digDamage = 1;
    [SerializeField] [Min(0)] private int money = 0;
    [SerializeField] private GameObject shovel = null;
    [SerializeField] private GameObject fxPrefab = null;
    [Header("Walk")]
    [SerializeField] private WalkingPath walkingPath = null;
    [SerializeField] private float pathDuration = 15f;
    [SerializeField] private PathType pathType = PathType.Linear;

    [Header("Dive")]
    [SerializeField] private WalkingPath divingPath = null;
    [SerializeField] private float divingPathDuration = .56f;
    [SerializeField] private PathType divingPathEase = PathType.Linear;
    [Header("Swim")]
    [SerializeField] private int breathTime = 20;
    [SerializeField] private float swimSpeed = 6f, sideSwimSpeed = 8f;
    private void Start() {
        this.gameManager = GameManager.Instance;
        this.swimBehaviour = this.gameManager.SwimBehaviour;
        this.animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator TimerBreath(float breathTime = 20f) {
        this.gameManager.UIPanUP(this.gameManager.Digging);
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
        StartCoroutine(SetScore(this.dugLayers));
        this.transform.DOPath(this.walkingPath.Waypoints, this.pathDuration, this.pathType, PathMode.Sidescroller2D, 10, Color.blue)
             .OnWaypointChange((int index) => {
                 ResetAllBoolAnimator();
                 if(index == 1) {
                     this.animator.SetTrigger("Walk");
                 } else if(index == 5) {
                     this.animator.SetTrigger("Climb");
                 } else if(index == 6) {
                     this.animator.SetTrigger("Walk");
                 }
                 if(index + 1 < this.walkingPath.Waypoints.Length) {
                     Vector3 towards = new Vector3(this.walkingPath.Waypoints[index + 1].x, this.walkingPath.Waypoints[index].y, this.walkingPath.Waypoints[index + 1].z);
                     this.transform.DOLookAt(towards, .4f);
                     Debug.DrawLine(this.walkingPath.Waypoints[index], towards, Color.red, 1f);
                 }
             }
         ).SetEase(Ease.Linear).OnComplete(() => this.animator.SetTrigger("Idle"));
    }

    private void ResetAllBoolAnimator() {
        foreach(AnimatorControllerParameter parameter in this.animator.parameters) {
            this.animator.SetBool(parameter.name, false);
        }
    }


    private void Update() {
        if(this.state == PlayerState.Swim && this.transform.position.y <= -this.gameManager.Pool.Depth) {
            this.transform.DOMoveY(-this.gameManager.Pool.Depth, 0, true);
            SetState(PlayerState.Dig);
        }
        if(this.state == PlayerState.Idle) {
            if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) || this.isAutoClicker) {
                ResetAllBoolAnimator();
                this.animator.SetTrigger("Dive");
            }
        }
    }

    public void Dive() {
        this.transform.DOPath(this.divingPath.Waypoints, this.divingPathDuration, this.divingPathEase, PathMode.Full3D, 10, Color.blue)
            .OnWaypointChange((int index) => {
                if(index + 1 == this.divingPath.Waypoints.Length) {
                    SetState(PlayerState.Swim);
                }
            }).SetEase(Ease.Linear);
    }

    public void Dig(int? digDamage = null) {
        if(!this.hasDug) {
            StartCoroutine(TimerBreath(this.breathTime));
            this.gameManager.WaterPoolGo.SetActive(true);
            this.hasDug = true;
        }
        PoolDepthBehaviour pool = this.gameManager.Pool;
        Instantiate(this.fxPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("Dig");
        if(pool.Dig(digDamage == null ? this.digDamage : digDamage.Value)) {
            this.dugLayers++;
        }
        this.animator.SetTrigger("Dig");
    }

    private IEnumerator SetScore(int amount) {
        while(this.dugLayers > 0) {
            FindObjectOfType<AudioManager>().Play("calcul");
            yield return new WaitForSecondsRealtime(2f / amount);
            this.dugLayers--;
            AddMoney(1);
        }
        FindObjectOfType<AudioManager>().Stop("calcul");
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

    public void AddSwimSpeed(int amount) {
        this.swimSpeed += amount;
    }

    public void SetState(PlayerState state) {
        print($"Changed state from {this.state} to {state}");
        switch(this.state) {
            case PlayerState.Idle:
                break;
            case PlayerState.Swim:
                this.transform.DORotate(new Vector3(0, 135, 0), 0);
                this.swimBehaviour.gameObject.SetActive(false);
                break;
            case PlayerState.Dig:
                ResetAllBoolAnimator();
                this.animator.SetBool("DigMode", false);
                this.shovel.transform.DOScale(0, .2f);
                this.gameManager.UIPanDown(this.gameManager.Digging);
                this.gameManager.ClickableArea.onClick.RemoveAllListeners();
                this.gameManager.ClickableArea.interactable = false;
                if(this.autoclick != null) {
                    StopCoroutine(this.autoclick);
                }
                break;


            case PlayerState.GainMoney:
                GainMoney();
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
                ResetAllBoolAnimator();
                StartCoroutine(TimerBreath(this.breathTime));
                this.swimBehaviour.gameObject.SetActive(true);
                this.transform.DORotate(new Vector3(180, 135, 0), 0);
                FindObjectOfType<AudioManager>().Play("Swim");
                break;
            case PlayerState.Dig:
                ResetAllBoolAnimator();
                this.animator.SetBool("DigMode", true);
                this.shovel.transform.DOScale(1, .2f);
                this.gameManager.ClickableArea.onClick.AddListener(() => Dig());
                this.gameManager.ClickableArea.interactable = true;
                if(this.isAutoClicker) {
                    this.autoclick = StartCoroutine(AutoClick());
                }
                break;
            case PlayerState.GainMoney:
                this.transform.DOMove(this.walkingPath.Waypoints[0], 2f).OnComplete(() => SetState(PlayerState.Buy)).SetEase(Ease.Linear);
                Vector3 towards = new Vector3(this.walkingPath.Waypoints[0].x, this.transform.position.y, this.walkingPath.Waypoints[0].z);
                this.transform.DOLookAt(towards, .4f);
                break;
            case PlayerState.Buy:
                this.animator.SetTrigger("Climb");
                if(!this.isAutoClicker) {
                    this.gameManager.UIPanUP(this.gameManager.Store);
                } else {
                    SetState(PlayerState.Idle);
                }
                GameManager.Instance.Palier.CheckPalier();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject.Destroy(other.gameObject);
        AddMoney(GameManager.Instance.AddMoney);
    }

    public void SetAutoClicker(bool activate = false) {
        this.isAutoClicker = activate;
        if(this.state == PlayerState.Dig) {
            this.autoclick = StartCoroutine(AutoClick());
        }
    }

    public void SetAutoClicker(bool isActive = false, int digDamageToAdd = 0, bool improveFrequency = false) {
        this.isAutoClicker = isActive;
        this.autoClickerDigDamage += digDamageToAdd;
        this.autoClickerFrequency /= improveFrequency ? 2 : 1;
        if(this.state == PlayerState.Dig) {
            this.autoclick = StartCoroutine(AutoClick());
        }

    }

    private IEnumerator AutoClick() {
        while(true) {
            yield return new WaitForSecondsRealtime(this.autoClickerFrequency);
            Dig(this.autoClickerDigDamage);
        }
    }

    private Animator animator = null;
    private PlayerState state = PlayerState.Dig;
    private SwimBehaviour swimBehaviour = null;
    private GameManager gameManager = null;
    private int dugLayers = 0;
    private bool hasDug = false;

    //autoclicker fields
    private Coroutine autoclick = null;

}
public enum PlayerState { Idle, Swim, Dig, GainMoney, Buy }
