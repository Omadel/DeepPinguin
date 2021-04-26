using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFx : MonoBehaviour
{
    [SerializeField] private GameObject fxPrefab = null;
    

    private void OnEnable()
    {
        Instantiate(fxPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
    }

}
