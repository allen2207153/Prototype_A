using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchTrigger : MonoBehaviour, IPlayerTriggerAction
{
    private ButtonHintController buttonHintController;
    
    void Start()
    {
        buttonHintController = GetComponent<ButtonHintController>();
    }
    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetCrouchTrigger(true);
       // buttonHintController.SetButtonPrompt(false);
    }
    public void EndAction(PlayerMovement playerMovement)
    {
        playerMovement.SetCrouchTrigger(false);
    }

    private void OnDrawGizmos()
    {
        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.red; // Triggerの箱の色は赤とする
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
