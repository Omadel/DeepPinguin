using DG.Tweening;
using UnityEngine;

public class TestPlayer : MonoBehaviour {
    public GameObject go;
    public TMPro.TextMeshProUGUI text;
    public Color layerHurtColor;

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        if(Input.GetKey(KeyCode.Space)) {
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
