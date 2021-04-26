using UnityEngine;

public class StoreElement : MonoBehaviour {

    [SerializeField] private StoreElementStats stats = null;
    [SerializeField] public int AddPrice =1 ;
    [SerializeField] public int Level=1;
    [SerializeField] private GameManager gameManager = null;

    private void Start()
    {
        this.texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        RefreshText(this.stats.Name, this.stats.Cost.ToString(),this.Level.ToString());
    }

    private void RefreshText(string name, string cost,string level)
    {
        texts[0].text = name;
        texts[1].text = cost;
        texts[2].text = level;
    }

    public void Buy() {
        GameManager.Instance.Buy(this.stats, this);
        RefreshText(this.stats.Name, (this.stats.Cost * AddPrice).ToString() , this.Level.ToString());
    }
    
    TMPro.TextMeshProUGUI[] texts = null;

}
