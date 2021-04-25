using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SpawnMeshEffect : MonoBehaviour {
    [SerializeField] private float animationDelay = .01f;
    [SerializeField] private float animationTimer = .2f;
    [SerializeField] private AnimationCurve animationCurve = null;
    [SerializeField] private Ease animationEase = Ease.OutQuad;
    [SerializeField] private GameObject fxPrefab = null;

    private void OnEnable() {
        StartCoroutine(DelayedSpawn(this.animationDelay));
    }

    private IEnumerator DelayedSpawn(float delay = .01f) {
        foreach(Transform child in this.transform) {
            child.localScale = Vector3.zero;
            if(this.animationCurve.length < 2) {
                child.DOScale(1, this.animationTimer).SetEase(this.animationEase);
            } else {
                child.DOScale(1, this.animationTimer).SetEase(this.animationCurve);
            }
            if(delay > 0) {
                yield return new WaitForSecondsRealtime(delay);
            }
        }

    }
}
