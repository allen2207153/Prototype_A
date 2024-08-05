using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerActionSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerEvent.CallCheckCollider(other, true);
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerEvent.CallCheckCollider(other, false);
    }
}
