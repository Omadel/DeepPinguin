using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DigBehaviour))]
public class PlayerBehaviour : MonoBehaviour {

    public int DigDamage { get => this.digDamage; set => this.digDamage = value; }
    public int Money { get => this.money; set => this.money = value; }
    public int BreathTime { get => this.breathTime; set => this.breathTime = value; }
    public float SwimSpeed { get => this.swimSpeed; }
    public float SideSwimSpeed { get => this.sideSwimSpeed; }
    public PlayerState State { get => this.state; }
    public GameObject Shovel { get => this.shovel; }

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
        this.ploufVFX = LoadVFX("VFX_Plouf", true, false, this.transform.position);
        this.bubblesVFX = LoadVFX("VFX_Bubbles", true, false, this.transform.position + Vector3.up, this.transform);
        this.finsVFX = LoadVFX("VFX_Fins", true, false, this.transform.position, this.transform);
        this.digBehaviour = GetComponent<DigBehaviour>();
        this.animator = GetComponentInChildren<Animator>();
        if(this.gm.Pool.Depth > 0) {
            this.gm.WaterPoolGo.SetActive(true);
        }
    }

    private GameObject LoadVFX(string vfxName, bool instantiate = false, bool spawnActivated = false, Vector3? position = null, Transform parent = null) {
        GameObject vfx = Resources.Load($"Prefabs/VFX/{vfxName}") as GameObject;
        if(instantiate) {
            vfx = Instantiate(vfx, position.Value, Quaternion.identity, parent);
            vfx.SetActive(spawnActivated);
        }
        return vfx;
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
        ChangeState(PlayerState.SwimBackUp);
    }

    private void WalkAlongPath() {
        StartCoroutine(SetScore(this.dugLayers));
        this.transform.DOPath(this.walkingPath.Waypoints, this.pathDuration, this.pathType, PathMode.Sidescroller2D, 10, Color.blue)
             .OnWaypointChange((int index) => {
                 ResetAllBoolAnimator();
                 if(index == 0) {
                     this.animator.SetTrigger("Climb");
                 } else if(index == 1) {
                     this.animator.SetTrigger("Walk");
                 } else if(index == 4) {
                     //this.gm.UIPanUP(this.gm.UI.Store);
                 } else if(index == 5) {
                     this.animator.SetTrigger("Climb");
                 } else if(index == 6) {
                     this.animator.SetTrigger("Walk");
                 }
                 if(index + 1 < this.walkingPath.Waypoints.Length) {
                     this.transform.DOLookAt(this.walkingPath.Waypoints[index + 1], .2f, AxisConstraint.Y, Vector3.up);
                 }
             }
         ).SetEase(Ease.Linear).OnComplete(() => ChangeState(PlayerState.Idle));
    }

    private void ResetAllBoolAnimator() {
        foreach(AnimatorControllerParameter parameter in this.animator.parameters) {
            this.animator.SetBool(parameter.name, false);
        }
    }


    public void Dive() {
        this.transform.DOPath(this.divingPath.Waypoints, this.divingPathDuration, this.divingPathEase, PathMode.Full3D, 10, Color.blue)
            .OnComplete(() => ChangeState(PlayerState.SwimDown)).SetEase(Ease.Linear);
    }

    public void Dig(int? digDamage = null) {
        if(this.gm.Pool.Depth == 0) {
            print("Has dug for the first time");
            StartCoroutine(TimerBreath(this.breathTime));
            this.gm.WaterPoolGo.SetActive(true);
            this.bubblesVFX.SetActive(true);
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

    public void ChangeState(PlayerState state) {
        print($"Changed state from <color=red>{this.state}</color> to <color=green>{state}</color>");
        ResetAllBoolAnimator();
        switch(this.state) {
            case PlayerState.Idle:
                this.gm.UI.ClickableArea.gameObject.SetActive(false);
                this.gm.UI.ClickableArea.onClick.RemoveAllListeners();
                break;
            case PlayerState.SwimDown:
                this.transform.DORotate(new Vector3(0, 135, 0), 0);
                this.swimBehaviour.gameObject.SetActive(false);
                this.ploufVFX.SetActive(false);
                this.finsVFX.SetActive(false);
                break;
            case PlayerState.Dig:
                this.digBehaviour.enabled = false;
                //todo autoclick
                //if(this.autoclick != null) {
                //    StopCoroutine(this.autoclick);
                //}
                break;
            case PlayerState.SwimBackUp:

                break;
            case PlayerState.Walk:
                break;
            default:
                break;
        }
        this.state = state;
        switch(state) {
            case PlayerState.Idle:
                this.animator.SetTrigger("Idle");
                this.gm.UI.ClickableArea.gameObject.SetActive(true);
                this.gm.UI.ClickableArea.onClick.AddListener(() => this.animator.SetTrigger("Dive"));
                break;
            case PlayerState.SwimDown:
                StartCoroutine(TimerBreath(this.breathTime));
                this.swimBehaviour.gameObject.SetActive(true);
                this.transform.DORotate(new Vector3(180, 135, 0), 0);
                this.ploufVFX.SetActive(true);
                this.bubblesVFX.SetActive(true);
                this.finsVFX.SetActive(true);
                //todo correct audio manager
                FindObjectOfType<AudioManager>().Play("Swim");
                break;
            case PlayerState.Dig:
                this.digBehaviour.enabled = true;
                //todo autoclicker
                //if(this.isAutoClicker) {
                //    this.autoclick = StartCoroutine(AutoClick());
                //}
                break;
            case PlayerState.SwimBackUp:
                //todo system to climb back to the surface
                this.transform.DOMove(this.walkingPath.Waypoints[0], 2f).OnComplete(() => ChangeState(PlayerState.Walk)).SetEase(Ease.Linear);
                this.transform.DOLookAt(this.walkingPath.Waypoints[0], .4f, AxisConstraint.Y);
                this.finsVFX.SetActive(true);
                this.bubblesVFX.SetActive(false);
                break;
            case PlayerState.Walk:
                WalkAlongPath();
                this.gm.Echelons.CheckEchelons();
                this.finsVFX.SetActive(false);
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
    private DigBehaviour digBehaviour;
    private GameManager gm;
    private int dugLayers;
    private GameObject ploufVFX, bubblesVFX, finsVFX;

    //autoclicker fields
    private Coroutine autoclick;

}
public enum PlayerState { Idle, SwimDown, Dig, SwimBackUp, Walk }
