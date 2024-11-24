using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Transform[] _interactPoints;

    public Transform GetInteractPoint(Transform playerTransform)
    {
        Transform interactPoint = null;

        float shortestDistance = float.PositiveInfinity;

        foreach (var point in _interactPoints)
        {
            float distance = Vector3.Distance(point.position, playerTransform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                interactPoint = point;
            }
        }

        return interactPoint;
    }

}
