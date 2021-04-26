using UnityEngine;

public class StoreElement : MonoBehaviour {

    [SerializeField] private StoreElementStats stats = null;
    [SerializeField] public int AddPrice =1 ;
    [SerializeField] public int Level=1;
    [SerializeField] private GameManager gameManager = null;

    private void Start()
    {
        this.texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        RefreshText(this.stats.Name, this.stats.Cost.ToString());
    }

    private void RefreshText(string name, string cost)
    {
        texts[0].text = name;
        texts[1].text = cost;
    }

    public void Buy() {
        GameManager.Instance.Buy(this.stats, this);
        RefreshText(this.stats.Name, (this.stats.Cost * AddPrice).ToString() );
    }
    
    TMPro.TextMeshProUGUI[] texts = null;

}
