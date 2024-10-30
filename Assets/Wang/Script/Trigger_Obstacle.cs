using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trigger_Obstacle : MonoBehaviour, IPlayerTriggerAction
{
    public ObjectFallController objectFallController;
    private bool playerInRange = false;
    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetInteractionTrigger(true);
        // buttonHintController.SetButtonPrompt(false);
    }
    public void EndAction(PlayerMovement playerMovement)
    {
        playerMovement.SetInteractionTrigger(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = true;
            
        }
    
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = false;
        }
    }

    public void interaction(InputAction.CallbackContext _ctx)
    {
        if (playerInRange && _ctx.phase == InputActionPhase.Started)
        {
            objectFallController.TriggerFall(); // 触发目标物件倒下
           
        }
    }
    private void OnDrawGizmos()
    {
        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow; 
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }


}
