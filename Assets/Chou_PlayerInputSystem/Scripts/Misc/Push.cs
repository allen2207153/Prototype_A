using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Push : MonoBehaviour
{
    //[SerializeField] float _grabDistance = 1.5f;
    [SerializeField] float _pushForce = 2f;
    [SerializeField] float _slowMoveFactor = 0.5f; // 押している時や引っ張っている時速度のレート
    //[SerializeField] bool _detectingMovableBox = false; //　移動できる箱を検出のフラッグ

    public LayerMask _movableLayer;
    public Vector3 _hitNormal;
    public float _pushAngle = 45f;

    public float _detectHeight = 0.5f;
    public float _checkDistance = 1;

    private bool _isInteracting = false;
    private GameObject _currentBox;
    private Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (_isInteracting)
        {
            Rigidbody boxRigidbody = _currentBox.GetComponent<Rigidbody>();
            Vector3 direction = transform.forward * _player.GetCurrentMoveInput().y + transform.right * _player.GetCurrentMoveInput().x;
            float distance = Vector3.Distance(transform.position, boxRigidbody.position);

            if (distance < 3)
            {
                if (_player.GetCurrentMoveInput().y < 0)
                {
                    _pushForce = 3f;
                }
                else
                {
                    _pushForce = 2f;
                }
                boxRigidbody.velocity = direction * _pushForce;
            }
        }
    }

    public void StartGrab(Transform playerTransform, Vector3 inputDirection)
    {
#if false
        Collider[] _hitColliders = Physics.OverlapSphere(transform.position, _grabDistance);
        foreach (var hitCollider in _hitColliders)
        {
            if (hitCollider.CompareTag("Box"))
            {
                _currentBox = hitCollider.gameObject;
                _isInteracting = true;
                _player.MoveHands(true);
                break;
            }
        }
#endif

        _currentBox = MovableBoxDetect(playerTransform, inputDirection);
        if (_currentBox != null)
        {
            _isInteracting = true;
            _player.MoveHands(true);
        }

    }

    public void StopGrab()
    {
        _isInteracting = false;
        _currentBox = null;
        _player.MoveHands(false);
    }

    public bool IsInteracting()
    { 
        return _isInteracting;
    }

    public float GetSlowMoveFactor() 
    { 
        return _slowMoveFactor;
    }

    // 押せる箱があるかを検出する
    public GameObject MovableBoxDetect(Transform playerTransform, Vector3 inputDirection)
    {
        Debug.DrawLine(playerTransform.position + Vector3.up * _detectHeight,
            playerTransform.position + Vector3.up * _detectHeight + playerTransform.forward * _checkDistance,
            UnityEngine.Color.red);
        if (Physics.Raycast(playerTransform.position + Vector3.up * _detectHeight,
            playerTransform.forward, out RaycastHit hit, _checkDistance, _movableLayer))
        {
            _hitNormal = hit.normal;
            if (Vector3.Angle(-_hitNormal, playerTransform.forward) > _pushAngle
                || Vector3.Angle(-_hitNormal, inputDirection) > _pushAngle)
            {
                return null;
            }
            return hit.collider.gameObject;
        }
        //Debug.Log("NO BOX");
        return null;
    }

}
