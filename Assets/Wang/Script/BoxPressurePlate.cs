using UnityEngine;

public class BoxPressurePlate : PressurePlate
{
    void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.CompareTag("Box"))
        {
            isActivated = true;
            Activate();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isActivated && other.CompareTag("Box"))
        {
            isActivated = false;
            Deactivate();
        }
    }

    public override void Activate()
    {
        Debug.Log("箱が感圧地板を起動しました。");
        StartCoroutine(Sink());
    }

    public override void Deactivate()
    {
        Debug.Log("箱が感圧地板を解除しました。");
        StartCoroutine(ReturnToInitialPosition());
    }
}