using UnityEngine;

public class SinglePlayerPressurePlate : PressurePlate
{
    void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            Activate();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isActivated && other.CompareTag("Player"))
        {
            isActivated = false;
            Deactivate();
        }
    }

    public override void Activate()
    {
        Debug.Log("単独プレイヤーが感圧地板を起動しました。");
        StartCoroutine(Sink());
    }

    public override void Deactivate()
    {
        Debug.Log("単独プレイヤーが感圧地板を解除しました。");
        StartCoroutine(ReturnToInitialPosition());
    }
}
