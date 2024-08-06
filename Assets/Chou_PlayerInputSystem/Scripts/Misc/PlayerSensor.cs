using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public LayerMask _movableLayer;

    [Header("箱が検出できる距離")]
    public float _checkDistance = 1f;
    
    public float _pushAngle = 45f;
    public Vector3 _pushHitNormal;

    [Header("プレイヤが押す箱の最低高さ")]
    public float _movableObjectHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MovableObject MovableObjectCheck(Transform playerTransform)
    {
        if (Physics.Raycast(playerTransform.position + Vector3.up * _movableObjectHeight, playerTransform.forward,
            out RaycastHit hit, _checkDistance, _movableLayer))
        {
            _pushHitNormal = hit.normal;
            if (Vector3.Angle(-_pushHitNormal, playerTransform.forward) > _pushAngle)
            {
                return null;
            }
            MovableObject movableObject;
            if (hit.collider.TryGetComponent<MovableObject>(out movableObject))
            {
                return movableObject;
            }
        }
        return null;
    }
}
