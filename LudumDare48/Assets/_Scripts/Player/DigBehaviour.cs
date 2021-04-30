using DG.Tweening;
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

    }

    private void OnDisable() {
        this.player.Shovel.transform.DOScale(0, .2f);
        GameManager gm = GameManager.Instance;
        gm.UIPanDown(gm.UI.Digging);
        gm.UI.ClickableArea.gameObject.SetActive(false);
        gm.UI.ClickableArea.onClick.RemoveAllListeners();
    }

    private PlayerBehaviour player;
    private Animator animator;
}
