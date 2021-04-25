using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palier_apparition : MonoBehaviour
{
    public PlayerBehaviour player;
    public GameObject test;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( player.Money == 10)
        {
            test.SetActive(true);
        }
    }
}
