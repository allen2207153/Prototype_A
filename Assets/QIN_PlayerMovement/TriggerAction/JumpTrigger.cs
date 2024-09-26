using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour,IPlayerTriggerAction
{
    private ControllerVibration controllerVibration;

    void Start()
    {
        controllerVibration = GetComponent<ControllerVibration>();
    }
    public void TriggerAction(PlayerMovement playerMovement)
    {
        playerMovement.SetJumpTrigger(true);
        controllerVibration.StartVibration(0.3f, 0.3f, 0.5f); // 1秒間、低・高周波数で振動
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
