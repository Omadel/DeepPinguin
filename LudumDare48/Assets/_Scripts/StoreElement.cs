using UnityEngine;

public class StoreElement : MonoBehaviour {

    [SerializeField] private StoreElementStats stats = null;

    private void Start() {
        TMPro.TextMeshProUGUI[] texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        texts[0].text = this.stats.Name;
        texts[1].text = this.stats.Cost.ToString();
    }

    public void Buy() {
        GameManager.Instance.Buy(this.stats);
    }
}
