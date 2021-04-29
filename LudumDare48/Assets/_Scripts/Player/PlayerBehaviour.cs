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
    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject fxPrefab;
    [Header("Walk")]
    [SerializeField] private WalkingPath walkingPath;
    [SerializeField] private float pathDuration = 15f;
    [SerializeField] private PathType pathType = PathType.Linear;

    [Header("Dive")]
    [SerializeField] private WalkingPath divingPath;
    [SerializeField] private float divingPathDuration = .56f;
    [SerializeField] private PathType divingPathEase = PathType.Linear;
    [Header("Swim")]
    [SerializeField] private int breathTime = 20;
    [SerializeField] private float swimSpeed = 6f, sideSwimSpeed = 8f;
    private void Start() {
        this.gm = GameManager.Instance;
        this.swimBehaviour = this.gm.UI.SwimBehaviour;
        this.animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator TimerBreath(float breathTime = 20f) {
        this.gm.UIPanUP(this.gm.UI.Digging);
        float timeLeft = breathTime;
        this.gm.UI.BreathBar.maxValue = timeLeft;
        this.gm.UI.BreathBar.DOValue(timeLeft, .5f);
        while(timeLeft >= 0) {
            this.gm.UI.BreathBar.DOValue(timeLeft, .5f);
            timeLeft--;
            yield return new WaitForSecondsRealtime(1);
        }
        SetState(PlayerState.GainMoney);
    }

    private void GainMoney() {
        StartCoroutine(SetScore(this.dugLayers));
        this.transform.DOPath(this.walkingPath.Waypoints, this.pathDuration, this.pathType, PathMode.Sidescroller2D, 10, Color.blue)
             .OnWaypointChange((int index) => {
                 ResetAllBoolAnimator();
                 if(index == 0) {
                     this.animator.SetTrigger("Climb");
                 } else if(index == 1) {
                     this.animator.SetTrigger("Walk");
                 } else if(index == 5) {
                     this.animator.SetTrigger("Climb");
                 } else if(index == 6) {
                     this.animator.SetTrigger("Walk");
                 }
                 if(index + 1 < this.walkingPath.Waypoints.Length) {
                     this.transform.DOLookAt(this.walkingPath.Waypoints[index + 1], .2f, AxisConstraint.Y, Vector3.up);
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
        if(this.state == PlayerState.Swim && this.transform.position.y <= -this.gm.Pool.Depth) {
            this.transform.DOMoveY(-this.gm.Pool.Depth, 0, true);
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
            .OnComplete(() => SetState(PlayerState.Swim)).SetEase(Ease.Linear);
    }

    public void Dig(int? digDamage = null) {
        if(!this.hasDug) {
            print("Has dug for the fiorst time");
            StartCoroutine(TimerBreath(this.breathTime));
            this.gm.WaterPoolGo.SetActive(true);
            this.hasDug = true;
        }
        PoolDepthBehaviour pool = this.gm.Pool;
        GameObject.Destroy(Instantiate(this.fxPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity), 2f);
        //todo correct audio manager
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
            AddMoney(1 * this.gm.MoneyMultiplicator);
        }
        FindObjectOfType<AudioManager>().Stop("calcul");
    }

    public void AddBreathTime(int amount) {
        this.breathTime += amount;
    }

    public void AddMoney(int amount) {
        this.money += amount;
        this.gm.UI.ChangeText(this.gm.UI.MoneyText, this.money.ToString());
    }

    public void AddDigDamage(int amount) {
        this.digDamage += amount;
        this.gm.UI.ChangeText(this.gm.UI.DigDmgText, this.digDamage.ToString());
    }

    public void AddSwimSpeed(int amount) {
        this.swimSpeed += amount;
    }

    public void SetState(PlayerState state) {
        print($"Changed state from <color=red>{this.state}</color> to <color=green>{state}</color>");
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
                this.gm.UIPanDown(this.gm.UI.Digging);
                //this.gm.UI.ClickableArea.onClick.RemoveAllListeners();
                this.gm.UI.ClickableArea.gameObject.SetActive(false);
                if(this.autoclick != null) {
                    StopCoroutine(this.autoclick);
                }
                break;


            case PlayerState.GainMoney:
                GainMoney();
                break;
            case PlayerState.Buy:
                this.gm.UIPanDown(this.gm.UI.Store);
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
                //todo correct audio manager
                FindObjectOfType<AudioManager>().Play("Swim");
                break;
            case PlayerState.Dig:
                ResetAllBoolAnimator();
                this.animator.SetBool("DigMode", true);
                this.shovel.transform.DOScale(1, .2f);
                //this.gm.UI.ClickableArea.onClick.AddListener(() => Dig());
                this.gm.UI.ClickableArea.gameObject.SetActive(true);
                if(this.isAutoClicker) {
                    this.autoclick = StartCoroutine(AutoClick());
                }
                break;
            case PlayerState.GainMoney:
                //todo system to climb back to the surface
                this.transform.DOMove(this.walkingPath.Waypoints[0], 2f).OnComplete(() => SetState(PlayerState.Buy)).SetEase(Ease.Linear);
                this.transform.DOLookAt(this.walkingPath.Waypoints[0], .4f, AxisConstraint.Y);
                break;
            case PlayerState.Buy:
                if(!this.isAutoClicker) {
                    this.gm.UIPanUP(this.gm.UI.Store);
                } else {
                    SetState(PlayerState.Idle);
                }
                this.gm.Echelons.CheckEchelons();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject.Destroy(other.gameObject);
        AddMoney(1 * this.gm.MoneyMultiplicator);
    }

    public void ToggleAutoLock() {
        SetAutoClicker(!this.isAutoClicker);
    }

    public void SetAutoClicker(bool activate = false) {
        this.isAutoClicker = activate;
        if(this.state == PlayerState.Dig) {
            if(activate) {

                this.autoclick = StartCoroutine(AutoClick());
            } else {
                StopCoroutine(this.autoclick);
            }
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

    private Animator animator;
    private PlayerState state = PlayerState.Dig;
    private SwimBehaviour swimBehaviour;
    private GameManager gm;
    private int dugLayers;
    private bool hasDug;

    //autoclicker fields
    private Coroutine autoclick;

}
public enum PlayerState { Idle, Swim, Dig, GainMoney, Buy }
