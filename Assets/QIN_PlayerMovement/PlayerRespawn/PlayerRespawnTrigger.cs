using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("imouto"))
        {
            EventSystem.Instance.TriggerEvent(GameEvents.PlayerUpdateRespawnPoint);
        }
    }
}
