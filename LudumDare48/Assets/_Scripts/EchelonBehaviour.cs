using System.Collections.Generic;
using UnityEngine;

public class EchelonBehaviour : MonoBehaviour {
    public int Current { get => this.currentEchelon; }
    [SerializeField] private List<GameObject> echelons = new List<GameObject>();

    private void Start() {
        FetchEchelons();
        DisableAllEchelons();
    }

    private void FetchEchelons() {
        this.echelons.Clear();
        foreach(Transform child in this.transform) {
            this.echelons.Add(child.gameObject);
        }
    }

    private void DisableAllEchelons() {
        foreach(GameObject echelon in this.echelons) {
            echelon.SetActive(false);
        }
    }

    [ContextMenu("Check Echelons")]
    public void CheckEchelons() {

        while(GameManager.Instance.Pool.Depth >= 10 * (this.currentEchelon * this.currentEchelon) + 1 && this.currentEchelon < 33) {
            FindObjectOfType<AudioManager>().Play("Palier");
            this.echelons[this.currentEchelon].SetActive(true);
            this.currentEchelon++;

        }
        GameManager.Instance.UI.ChangeText(GameManager.Instance.UI.EchelonsText, this.currentEchelon.ToString());
    }

    private int currentEchelon;
}
