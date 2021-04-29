using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class FpsCounter : MonoBehaviour {
    [SerializeField] private float refresh = 1f;

    private void Start() {
        this.text = GetComponent<TMPro.TextMeshProUGUI>();
        StartCoroutine(SlowUpdate());
    }

    private IEnumerator SlowUpdate() {
        while(true) {
            float timelapse = Time.unscaledDeltaTime;
            this.text.text = ((int)(1f / timelapse)).ToString();
            yield return new WaitForSecondsRealtime(this.refresh);
        }
    }

    private TMPro.TextMeshProUGUI text;
}
