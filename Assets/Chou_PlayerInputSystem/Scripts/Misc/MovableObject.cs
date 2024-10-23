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

            // 高低差を考慮して、Y軸方向のオフセットを調整
            Vector3 correctedPoint = new Vector3(point.position.x, playerTransform.position.y, point.position.z);
            distance = Vector3.Distance(correctedPoint, playerTransform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                interactPoint = point;
            }
        }

        return interactPoint;
    }
    }
