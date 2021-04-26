using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PalierApparition : MonoBehaviour
{
    public List<GameObject> Palier;
    private int value = 0;
    public TextMeshProUGUI Level { get => this.LevelText; }

    [SerializeField] private TextMeshProUGUI LevelText = null;

    public void CheckPalier()
    {
        
        if (GameManager.Instance.Pool.Depth >= 10 * (value * value)+1 && value <33)
        {
            Palier[value].SetActive(true);
            value = value + 1;
        }
        LevelText.text = value.ToString() + "/ " + 33 ;
    }
}
