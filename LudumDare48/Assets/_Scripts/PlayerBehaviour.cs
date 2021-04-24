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
        GameManager.Instance.BreathBar.value = timeLeft;
        while(timeLeft >= 0) {
            yield return new WaitForSecondsRealtime(1);
            GameManager.Instance.BreathBar.value = timeLeft;
            timeLeft--;
        }
        this.state = PlayerState.Show;
        this.transform.DOMoveY(0, 2f).OnComplete(ActivateShow);
    }

    private void Update() {
        if(this.state == PlayerState.Dig) {
            if(Input.GetMouseButtonDown(0) || Input.touchCount > 0) {
                PoolDepthBehaviour pool = GameManager.Instance.Pool;
                pool.Dig(this.digDamage);
                this.transform.DOMoveY(-pool.Depth, pool.DigDuration).SetEase(pool.DigEase);
            }
        }
    }

    private void ActivateShow() {
        StartCoroutine(SetScore(GameManager.Instance.Pool.Depth));
    }

    private IEnumerator SetScore(int amount) {
        int score = int.Parse(GameManager.Instance.Money.text);
        while(score < amount) {
            yield return new WaitForSecondsRealtime(.1f);
            print(score);
            this.money++;
            score++;
            GameManager.Instance.Money.text = this.money.ToString();
        }
    }

    private enum PlayerState { Swim, Dig, Show }

    private PlayerState state = PlayerState.Dig;
}
