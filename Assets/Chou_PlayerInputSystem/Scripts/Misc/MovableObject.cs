using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public Transform[] _interactPoints;

    public bool _touchObstacle;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _touchObstacle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _touchObstacle = false;
        }
    }

}
