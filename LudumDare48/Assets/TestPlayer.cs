using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour {
    public GameObject go;
    public TMPro.TextMeshProUGUI text;
    public float refresh = 1f;
    public Color layerHurtColor;
    public Vector2 range;
    // Start is called before the first frame update
    private void Start() {
        StartCoroutine(bonjour());
    }

    private Vector3 spawnPos;

    private IEnumerator bonjour() {
        while(true) {
            float tmp = Random.Range(this.range.x, this.range.y);
            float mult = (tmp - this.range.x) / (this.range.y - this.range.x);
            float tmp2 = (this.range.y - this.range.x) * (1 - mult) + this.range.x;
            this.spawnPos = new Vector3(tmp, mult, -tmp2);

            yield return new WaitForSecondsRealtime(this.refresh);
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(new Vector3(this.spawnPos.x, 0, this.spawnPos.z), 1);
    }
    // Update is called once per frame
    private void Update() {
        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0) {
            this.text.text = (int.Parse(this.text.text) + 1).ToString();
            this.text.transform.DOComplete();
            this.text.DOComplete();
            this.text.transform.DOShakePosition(.3f, 3f);
            this.text.transform.DOShakeRotation(.3f, 3f);
            this.text.transform.DOPunchScale(Vector3.one * .2f, .3f);
            this.text.DOColor(Color.grey, .1f).SetLoops(2, LoopType.Yoyo);


        }
    }
}
