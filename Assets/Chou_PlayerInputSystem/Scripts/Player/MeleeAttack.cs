using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    private float _coldTime = 2f;

    [SerializeField]
    private float _attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
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

}
