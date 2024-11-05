using UnityEngine;

public class DoublePlayerPressurePlate : PressurePlate
{
    private int playerCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("imouto")) && playerCount < 2)
        {
            playerCount++;
            if (playerCount == 2 && !isActivated)
            {
                isActivated = true;
                Activate();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("imouto")) && playerCount > 0)
        {
            playerCount--;
            if (playerCount < 2 && isActivated)
            {
                isActivated = false;
                Deactivate();
            }
        }
    }

    public override void Activate()
    {
        Debug.Log("2人プレイヤーが感圧地板を起動しました。");
        StartCoroutine(Sink());
    }

    public override void Deactivate()
    {
        Debug.Log("2人プレイヤーが感圧地板を解除しました。");
        StartCoroutine(ReturnToInitialPosition());
    }
}