using DG.Tweening;
using UnityEngine;

public class SpawnMeshEffect : MonoBehaviour {
    [SerializeField] private float animationTimer = .2f;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Ease animationEase = Ease.OutQuad;
    [SerializeField] private GameObject fxPrefab = null;

    private void OnEnable() {
        foreach(Transform child in this.transform) {
            child.localScale = Vector3.zero;
            if(this.animationCurve.length < 2) {
                child.DOScale(1, this.animationTimer).SetEase(this.animationEase);
            } else {
                child.DOScale(1, this.animationTimer).SetEase(this.animationCurve);
            }
        }
    }
}
