using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DigBehaviour))]
public class PlayerBehaviour : MonoBehaviour {

    public int DigDamage { get => digDamage; set => digDamage = value; }
    public int Money { get => money; set => money = value; }
    public int BreathTime { get => breathTime; set => breathTime = value; }
    public float SwimSpeed { get => swimSpeed; }
    public float SideSwimSpeed { get => sideSwimSpeed; }
    public PlayerState State { get => state; }
    public GameObject Shovel { get => shovel; }
    public bool IsAutoCliker { get => isAutoCliker; }
    public int AutoClickerDigDamage { get => aCDigDamage; }
    public float AutoClickerFrequency { get => aCFrequency; }

    [Header("Auto clicker")]
    [SerializeField] private int aCDigDamage = 0;
    [SerializeField] private float aCFrequency = 3f;

    [Header("Player")]
    [SerializeField] private int digDamage = 1;
    [SerializeField] [Min(0)] private int money = 0;
    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject fxPrefab;
    [Header("Walk")]
    [SerializeField] private Etienne.Path walkingPath;
    [SerializeField] private float pathDuration = 15f;
    [SerializeField] private PathType pathType = PathType.Linear;

    [Header("Dive")]
    [SerializeField] private Etienne.Path divingPath;
    [SerializeField] private float divingPathDuration = .56f;
    [SerializeField] private PathType divingPathEase = PathType.Linear;
    [Header("Swim")]
    [SerializeField] private int breathTime = 20;
    [SerializeField] private float swimSpeed = 6f, sideSwimSpeed = 8f;

    private void Start() {
        gm = GameManager.Instance;
        swimBehaviour = gm.UI.SwimBehaviour;
        ploufVFX = LoadVFX("VFX_Plouf", true, false, transform.position);
        bubblesVFX = LoadVFX("VFX_Bubbles", true, false, transform.position + Vector3.up, transform);
        finsVFX = LoadVFX("VFX_Fins", true, false, transform.position, transform);
        digBehaviour = GetComponent<DigBehaviour>();
        animator = GetComponentInChildren<Animator>();
        if(gm.Pool.Depth > 0) {
            gm.WaterPoolGo.SetActive(true);
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
        gm.UIPanUP(gm.UI.Digging);
        float timeLeft = breathTime;
        gm.UI.BreathBar.maxValue = timeLeft;
        gm.UI.BreathBar.DOValue(timeLeft, .5f);
        while(timeLeft >= 0) {
            gm.UI.BreathBar.DOValue(timeLeft, .5f);
            timeLeft--;
            yield return new WaitForSecondsRealtime(1);
        }
        ChangeState(PlayerState.SwimBackUp);
    }

    private void WalkAlongPath() {
        StartCoroutine(SetScore(dugLayers));
        transform.DOPath(walkingPath.WorldWaypoints, pathDuration, pathType, PathMode.Sidescroller2D, 10, Color.blue)
             .OnWaypointChange((int index) => {
                 ResetAllBoolAnimator();
                 if(index == 0) {
                     animator.SetTrigger("Climb");
                 } else if(index == 1) {
                     animator.SetTrigger("Walk");
                 } else if(index == 4) {
                     //this.gm.UIPanUP(this.gm.UI.Store);
                 } else if(index == 5) {
                     animator.SetTrigger("Climb");
                 } else if(index == 6) {
                     animator.SetTrigger("Walk");
                 }
                 if(index + 1 < walkingPath.WorldWaypoints.Length) {
                     transform.DOLookAt(walkingPath.WorldWaypoints[index + 1], .2f, AxisConstraint.Y, Vector3.up);
                 }
             }
         ).SetEase(Ease.Linear).OnComplete(() => ChangeState(PlayerState.Idle));
    }

    private void ResetAllBoolAnimator() {
        foreach(AnimatorControllerParameter parameter in animator.parameters) {
            animator.SetBool(parameter.name, false);
        }
    }


    public void Dive() {
        transform.DOPath(divingPath.WorldWaypoints, divingPathDuration, divingPathEase, PathMode.Full3D, 10, Color.blue)
            .OnComplete(() => ChangeState(PlayerState.SwimDown)).SetEase(Ease.Linear);
    }

    public void Dig(int? digDamage = null) {
        if(gm.Pool.Depth == 0) {
            print("Has dug for the first time");
            StartCoroutine(TimerBreath(breathTime));
            gm.WaterPoolGo.SetActive(true);
            bubblesVFX.SetActive(true);
        }
        PoolDepthBehaviour pool = gm.Pool;
        GameObject.Destroy(Instantiate(fxPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity), 2f);
        //todo correct audio manager
        FindObjectOfType<AudioManager>().Play("Dig");
        if(pool.Dig(digDamage == null ? this.digDamage : digDamage.Value)) {
            dugLayers++;
        }
        animator.SetTrigger("Dig");
    }

    private IEnumerator SetScore(int amount) {
        FindObjectOfType<AudioManager>().Play("calcul");
        while(dugLayers > 0) {
            yield return new WaitForSecondsRealtime(2f / amount);
            dugLayers--;
            AddMoney(1 * gm.MoneyMultiplicator);
        }
        FindObjectOfType<AudioManager>().Stop("calcul");
    }

    public void AddBreathTime(int amount) {
        breathTime += amount;
    }

    public void AddMoney(int amount) {
        money += amount;
        gm.UI.ChangeText(gm.UI.MoneyText, money.ToString());
    }

    public void AddDigDamage(int amount) {
        digDamage += amount;
        gm.UI.ChangeText(gm.UI.DigDmgText, digDamage.ToString());
    }

    public void AddSwimSpeed(int amount) {
        swimSpeed += amount;
    }

    public void ChangeState(PlayerState state) {
        print($"Changed state from <color=red>{this.state}</color> to <color=green>{state}</color>");
        ResetAllBoolAnimator();
        switch(this.state) {
            case PlayerState.Idle:
                gm.UI.ClickableArea.gameObject.SetActive(false);
                gm.UI.ClickableArea.onClick.RemoveAllListeners();
                break;
            case PlayerState.SwimDown:
                transform.DORotate(new Vector3(0, 135, 0), 0);
                swimBehaviour.gameObject.SetActive(false);
                ploufVFX.SetActive(false);
                finsVFX.SetActive(false);
                break;
            case PlayerState.Dig:
                digBehaviour.enabled = false;
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
                animator.SetTrigger("Idle");
                gm.UI.ClickableArea.gameObject.SetActive(true);
                gm.UI.ClickableArea.onClick.AddListener(() => animator.SetTrigger("Dive"));
                break;
            case PlayerState.SwimDown:
                StartCoroutine(TimerBreath(breathTime));
                swimBehaviour.gameObject.SetActive(true);
                transform.DORotate(new Vector3(180, 135, 0), 0);
                ploufVFX.SetActive(true);
                bubblesVFX.SetActive(true);
                finsVFX.SetActive(true);
                //todo correct audio manager
                FindObjectOfType<AudioManager>().Play("Swim");
                break;
            case PlayerState.Dig:
                digBehaviour.enabled = true;
                break;
            case PlayerState.SwimBackUp:
                //todo system to climb back to the surface
                transform.DOMove(walkingPath.WorldWaypoints[0], 2f).OnComplete(() => ChangeState(PlayerState.Walk)).SetEase(Ease.Linear);
                transform.DOLookAt(walkingPath.WorldWaypoints[0], .4f, AxisConstraint.Y);
                finsVFX.SetActive(true);
                bubblesVFX.SetActive(false);
                break;
            case PlayerState.Walk:
                WalkAlongPath();
                gm.Echelons.CheckEchelons();
                finsVFX.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject.Destroy(other.gameObject);
        AddMoney(1 * gm.MoneyMultiplicator);
    }



    public void ImproveACFrequency() {
        aCFrequency /= 2;
    }

    public void AddACDigDamage(int amount) {
        aCDigDamage += amount;
    }

    public void ToggleAutoClick() {
        if(isAutoCliker) {
            DisableAutoclick();
        } else {
            EnableAutoclick();
        }
    }

    [ContextMenu("Enable AC")]
    public void EnableAutoclick() {
        if(digBehaviour.isActiveAndEnabled) {
            digBehaviour.EnableAutoclick();
        }
        gm.UI.ChangeText(gm.UI.Autoclick.GetComponentInChildren<TMPro.TextMeshProUGUI>(), "AutoClick On");
        isAutoCliker = true;
    }

    [ContextMenu("Disable AC")]
    public void DisableAutoclick() {
        if(digBehaviour.isActiveAndEnabled) {
            digBehaviour.DisableAutoclick();
        }
        gm.UI.ChangeText(gm.UI.Autoclick.GetComponentInChildren<TMPro.TextMeshProUGUI>(), "AutoClick Off");
        isAutoCliker = false;
    }


    private Animator animator;
    private PlayerState state = PlayerState.Dig;
    private SwimBehaviour swimBehaviour;
    private DigBehaviour digBehaviour;
    private GameManager gm;
    private int dugLayers;
    private GameObject ploufVFX, bubblesVFX, finsVFX;
    private bool isAutoCliker = false;


}
public enum PlayerState { Idle, SwimDown, Dig, SwimBackUp, Walk }
