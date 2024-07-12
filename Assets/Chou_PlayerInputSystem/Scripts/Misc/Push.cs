using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    [SerializeField] float _grabDistance = 1.5f;
    [SerializeField] float _pushForce = 2f;
    [SerializeField] float _slowMoveFactor = 0.5f; // 押している時や引っ張っている時速度のレート

    private bool _isInteracting = false;
    private GameObject _currentBox;
    private Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (_isInteracting || _currentBox != null)
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

    public void StartGrab()
    {
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

}
