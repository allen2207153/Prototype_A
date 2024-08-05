using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangTrigger : MonoBehaviour,IPlayerTriggerAction
{
    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetHangTrigger(true);
    }
    public void EndAction(PlayerMovement playerMovement)
    {
        playerMovement.SetHangTrigger(false);
    }

    private void OnDrawGizmos()
    {

        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.blue;


            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);


            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
