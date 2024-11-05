using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerActionSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerEvent.Instance.CallCheckCollider(other, true);
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerEvent.Instance.CallCheckCollider(other, false);
    }
}
