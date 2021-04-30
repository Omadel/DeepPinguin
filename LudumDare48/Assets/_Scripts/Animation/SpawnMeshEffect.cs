using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SpawnMeshEffect : MonoBehaviour {
    [SerializeField] private AnimationParametersScriptableObject animationParameters;
    [SerializeField] private GameObject fxPrefab = null;
    [SerializeField] private bool stayActivatedOnAwake = false;


    private void Awake() {
        foreach(Transform child in this.transform) {
            if(this.fxPrefab != null) {
                Instantiate(this.fxPrefab, new Vector3(child.position.x, child.transform.position.y, child.transform.position.z), Quaternion.identity, child.transform);
            }
        }
        if(!this.stayActivatedOnAwake) {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        foreach(Transform child in this.transform) {
            child.localScale = Vector3.zero;
        }
        StartCoroutine(DelayedSpawn(this.animationParameters.Delay));
    }


    private IEnumerator DelayedSpawn(float delay = .01f) {
        foreach(Transform child in this.transform) {
            child.localScale = Vector3.zero;
            if(this.animationParameters.Curve.length < 2) {
                child.DOScale(1, this.animationParameters.Duration).SetEase(this.animationParameters.Ease);
            } else {
                child.DOScale(1, this.animationParameters.Duration).SetEase(this.animationParameters.Curve);
            }
            if(delay > 0) {
                yield return new WaitForSecondsRealtime(delay);
            }
        }

    }
}
