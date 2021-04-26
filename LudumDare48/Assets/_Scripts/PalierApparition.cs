using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalierApparition : MonoBehaviour
{
    public List<GameObject> Palier;
    private int value = 0; 
    
    public void CheckPalier()
    {
        
        if (GameManager.Instance.Pool.Depth >= 10 * (value * value)+1)
        {
            Palier[value].SetActive(true);
            value = value + 1;
        }
    }
}
