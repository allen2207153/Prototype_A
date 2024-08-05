using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour,IPlayerTriggerAction
{

    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetJumpTrigger(true);
    }
    public void EndAction(PlayerMovement playerMovement)
    {
        playerMovement.SetJumpTrigger(false);
    }

    private void OnDrawGizmos()
    {

        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.green;


            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);


            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
