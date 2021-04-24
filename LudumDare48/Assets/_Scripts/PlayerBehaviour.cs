using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] private int digDamage = 1;
    [SerializeField] [Min(0)] private int money = 0;
    [SerializeField] private float breathTime = 20f;
    private void Start() {
        StartCoroutine(TimerBreath(this.breathTime));
    }

    private IEnumerator TimerBreath(float breathTime = 20f) {
        float timeLeft = breathTime;
        GameManager.Instance.BreathBar.maxValue = timeLeft;
        while(timeLeft >= 0) {
            print(timeLeft);
            GameManager.Instance.BreathBar.value = timeLeft;
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("End of timer");
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
            PoolDepthBehaviour pool = GameManager.Instance.Pool;
            pool.Dig(this.digDamage);
            this.transform.DOMoveY(-pool.Depth, pool.DigDuration).SetEase(pool.DigEase);
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 3) {
            this.transform.DOMoveY(0, 2f);
        }
    }
}
