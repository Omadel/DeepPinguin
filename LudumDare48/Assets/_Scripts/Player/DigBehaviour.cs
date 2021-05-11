using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DigBehaviour : MonoBehaviour {
    private void Awake() {
        this.player = GetComponent<PlayerBehaviour>();
        this.animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable() {
        this.player.Shovel.transform.DOScale(1, .2f);
        GameManager gm = GameManager.Instance;
        this.animator.SetBool("DigMode", true);
        this.player.transform.DOMoveY(-gm.Pool.Depth, 0, true);
        gm.UI.ClickableArea.gameObject.SetActive(true);
        gm.UI.ClickableArea.onClick.AddListener(() => gm.Dig());


        if(this.player.IsAutoCliker) {
            EnableAutoclick();
        }

    }

    private void OnDisable() {
        this.player.Shovel.transform.DOScale(0, .2f);
        GameManager gm = GameManager.Instance;
        gm.UIPanDown(gm.UI.Digging);
        gm.UI.ClickableArea.gameObject.SetActive(false);
        gm.UI.ClickableArea.onClick.RemoveAllListeners();


        StopAllCoroutines();
        this.coroutine = null;
    }


    private IEnumerator Autoclick() {
        Debug.Log("Autoclick <color=red>Enabled</color>");
        GameManager gm = GameManager.Instance;
        while(true) {
            yield return new WaitForSecondsRealtime(this.player.AutoClickerFrequency);
            gm.Dig(this.player.AutoClickerDigDamage);
        }
    }
    public void EnableAutoclick() {
        if(this.coroutine == null) {
            this.coroutine = StartCoroutine(Autoclick());
        }
    }

    public void DisableAutoclick() {
        if(this.coroutine != null) {
            StopCoroutine(this.coroutine);
            this.coroutine = null;
        }
    }

    private PlayerBehaviour player;
    private Animator animator;
    private Coroutine coroutine;

}
