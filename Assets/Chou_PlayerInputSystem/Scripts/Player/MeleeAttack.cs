using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;

    public bool _isAttacking;

    [SerializeField]
    private GameObject _attackBox;

    BoxCollider _attackBoxCollider;

    [SerializeField]
    private float _coldTime = 2f;

    [SerializeField]
    private float _attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        _isAttacking = GetComponent<PlayerMovement>()._attackFlag;

        if (_attackBox != null)
        {
            _attackBoxCollider = _attackBox.GetComponent<BoxCollider>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _isAttacking = GetComponent<PlayerMovement>()._attackFlag;
        if (_attackTime <= 0)
        {
            if (_isAttacking)
            {
                // attack
                animator.SetTrigger("Attack");

                _attackTime = _coldTime;
            }
        }
        else
        {
            _attackTime -= Time.deltaTime;
        }
    }

    //Animation Event: Hit Start
    public void HitStart()
    {
        _attackBoxCollider.enabled = true;
    }

    //Animation Event: Hit End
    public void HitEnd()
    {
        _attackBoxCollider.enabled = false;
    }

}
