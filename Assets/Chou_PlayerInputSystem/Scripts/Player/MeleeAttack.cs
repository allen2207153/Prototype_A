using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;

    public bool _attackButtonDown;

    public bool _isAttacking;

    private BChara.Motion _motion;

    [SerializeField]
    private GameObject _attackBox;

    BoxCollider _attackBoxCollider;

    public float _damage = 5f;

    [SerializeField]
    private float _lightAttackDamage = 5f;

    [SerializeField]
    private float _heavyAttackDamage = 8f;

    [SerializeField]
    private float _coldTime = 1f;

    [SerializeField]
    private float _attackTime = 0;

    [SerializeField] private bool _isHoldingHand;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        _attackButtonDown = GetComponent<PlayerMovement>()._attackFlag;

        _motion = GetComponent<PlayerMovement>().GetMotion();

        if (_attackBox != null)
        {
            _attackBoxCollider = _attackBox.GetComponent<BoxCollider>();
        }

        _isHoldingHand = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag;

    }

    // Update is called once per frame
    void Update()
    {
        _attackButtonDown = GetComponent<PlayerMovement>()._attackFlag;
        _motion = GetComponent<PlayerMovement>().GetMotion();

        if (_attackTime <= 0)
        {
            if (_attackButtonDown && (_motion == BChara.Motion.Stand || _motion == BChara.Motion.Walk))
            {
                _isHoldingHand = GameObject.Find("Oniisan").GetComponent<PlayerMovement>()._grabHandFlag;
                // attack
                if ( _isHoldingHand )
                {
                    _damage = _lightAttackDamage;
                    animator.SetTrigger("Attack");
                    GameObject.Find("Oniisan").GetComponent<KnockBack>()._knockBackForce = 200f;
                    _coldTime = 0.67f;
                }
                else
                {
                    _damage = _heavyAttackDamage;
                    animator.SetTrigger("HeavyAttack");
                    GameObject.Find("Oniisan").GetComponent<KnockBack>()._knockBackForce = 300f;
                    _coldTime = 1f;
                }
                _isAttacking = true;

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

    //Animation Event: Attack End
    public void AttackEnd()
    {
        _isAttacking = false;
    }

}
