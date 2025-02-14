using Keto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainDisslove : MonoBehaviour
{
    private DissolveTest dissolve;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            dissolve.Dissolve();
            Debug.Log("disslove success");
        }
    }
}
