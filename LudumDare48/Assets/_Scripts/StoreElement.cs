using UnityEngine;

public class StoreElement : MonoBehaviour {

    [SerializeField] private StoreElementStats stats = null;

    // Start is called before the first frame update
    private void Start() {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"Buy : {this.stats.Name}";
    }

    // Update is called once per frame
    public void Buy() {
        GameManager.Instance.Buy(this.stats);
    }
}
