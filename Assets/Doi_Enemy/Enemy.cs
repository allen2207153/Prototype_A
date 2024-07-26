using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    //’Ç‚¢‚©‚¯‚é‘ÎÛ
    [SerializeField]
    private Transform _player;

    void Update()
    {
        _navMeshAgent.SetDestination(_player.position);
        //float distance = Vector3.Distance(transform.position, _player.position);
        //if (distance > GetComponent<NavMeshAgent>().stoppingDistance)
        //{

        //}
    }
}
