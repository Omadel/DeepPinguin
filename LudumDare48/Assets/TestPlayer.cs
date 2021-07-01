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
            float tmp = Random.Range(range.x, range.y);
            spawnPos = new Vector3(tmp, 0, -(range.y + range.x - tmp));

            yield return new WaitForSecondsRealtime(refresh);
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(new Vector3(spawnPos.x, 0, spawnPos.z), 1);
    }
    // Update is called once per frame
    private void Update() {
        if(Input.GetKey(KeyCode.Space) || Input.touchCount > 0) {
            text.text = (int.Parse(text.text) + 1).ToString();
            text.transform.DOComplete();
            text.DOComplete();
            text.transform.DOShakePosition(.3f, 3f);
            text.transform.DOShakeRotation(.3f, 3f);
            text.transform.DOPunchScale(Vector3.one * .2f, .3f);
            text.DOColor(Color.grey, .1f).SetLoops(2, LoopType.Yoyo);


        }
    }
}
