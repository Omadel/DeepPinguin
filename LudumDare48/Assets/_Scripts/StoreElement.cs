using UnityEngine;

public class StoreElement : MonoBehaviour {

    [SerializeField] private StoreElementStats stats = null;
    [SerializeField] public int AddPrice = 1;
    [SerializeField] public int Level = 1;

    private void Start() {
        this.texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        RefreshText(this.stats.Name, this.stats.Cost.ToString());
    }

    private void RefreshText(string name, string cost) {
        this.texts[0].text = name;
        this.texts[1].text = cost;
    }

    public void Buy() {
        GameManager.Instance.Buy(this.stats, this);
        RefreshText(this.stats.Name, (this.stats.Cost * this.AddPrice).ToString());
    }

    private TMPro.TextMeshProUGUI[] texts = null;

}
