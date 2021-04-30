using System.Collections;
using UnityEngine;

public class FpsCounter : MonoBehaviour {
    [SerializeField] private int targetFps = 60, okRange = 10;
    [SerializeField] private float refreshRate = 1f;

    private void Start() {
        TMPro.TextMeshProUGUI[] texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        this.currentDisplay = texts[0];
        this.averageDisplay = texts[1];
        this.maxDisplay = texts[2];
        this.minDisplay = texts[3];
        StartCoroutine(SlowUpdate());
    }

    private IEnumerator SlowUpdate() {
        yield return new WaitForSecondsRealtime(1);
        while(true) {
            float timelapse = Time.unscaledDeltaTime;
            int current = (int)(1f / timelapse);
            this.currentDisplay.text = $"FPS: {current}";
            this.currentDisplay.SetColor(current > this.targetFps - this.okRange);

            this.recordedAmount++;
            this.average += current;
            int avg = this.average / this.recordedAmount;
            this.averageDisplay.text = $"Avg: {avg}";
            this.averageDisplay.SetColor(avg > this.targetFps - this.okRange);

            this.max = current > this.max ? current : this.max;
            this.maxDisplay.text = $"Max: {this.max}";
            this.maxDisplay.SetColor(this.max > this.targetFps - this.okRange);

            this.min = current < this.min ? current : this.min;
            this.minDisplay.text = $"Min: {this.min}";
            this.minDisplay.SetColor(this.min > this.targetFps - this.okRange);

            yield return new WaitForSecondsRealtime(this.refreshRate);
        }
    }


    private TMPro.TextMeshProUGUI currentDisplay, averageDisplay, maxDisplay, minDisplay;
    private int recordedAmount, average, max, min = 500;
}

internal static class Extension {
    private static Color normalColor = new Color(51 / 255f, 204 / 255f, 51 / 255f),
        lowColor = new Color(255 / 255f, 80 / 255f, 80 / 255f);
    public static void SetColor(this TMPro.TextMeshProUGUI text, bool isOK) {
        text.color = isOK ? normalColor : lowColor;
    }
}
