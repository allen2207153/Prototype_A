using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    public Vector3 moveDirection = Vector3.zero;
    public float gravity = 8;
    public float rotateForce = 5; //��]��
    public float runForce = 2.5f; //�O�i��
    public float maxRunSpeed = 2; //�O�i���x�̐���
    public float jumpforce = 5; //�W�����v��
    public float mouseSensitivity = 2.0f; //�J�����X�s�[�h
    bool jumpableFlag = false;  //�n�ʂɂ��Ă��邩�ǂ���
    float jumpableCount = 0.0f; public float x = 0.2f;  //�W�����v�\�Ȏ��� 
    Vector3 defaultPosition;

    Quaternion defaultCameraDir;    //�f�t�H���g�̃J�����ʒu
    Vector3 defaultCameraOffset;    //�f�t�H���g�̃J�����ʒu�␳
    float charaDir = 0;             //�L�����N�^�[�̕���

    void Start()
    {
        Application.targetFrameRate = 60;
        // �K�v�ȃR���|�[�l���g�������擾
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
       // defaultCameraDir = Camera.main.transform.rotation;
       // defaultCameraOffset = Camera.main.transform.position - transform.position;
        defaultPosition = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Item")
        {
            Vector3 midlPosition = GameObject.Find("midl").transform.position;
            defaultPosition = midlPosition;
            Destroy(hit.gameObject);
        }
    }

    void Update()
    {
       if (controller == null) return;  //�L�����R���g���[���[�������Ă��Ȃ��ꍇ�͏I��
                                         //�������̓��͂ŕ����]������
                                         // transform.Rotate(0, Input.GetAxis("Horizontal") * rotateForce, 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = defaultPosition;
            moveDirection = new Vector3(0, 0, 0);
            Camera.main.transform.position = transform.position
                + Quaternion.Euler(0, charaDir, 0) * defaultCameraOffset;
            return;
        }

        //�W�����v
        if (controller.isGrounded)  //�n�ʂɒ��n���Ă�����
        {
            jumpableFlag = true;
            jumpableCount = x;
        }
        else
        {
            jumpableCount -= Time.deltaTime;
        }

        if (jumpableFlag && jumpableCount > 0.0f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpableFlag = false;
                moveDirection.y = jumpforce;
               
            }
        }


        //�J������]
        /*if(Input.GetKey(KeyCode.Z))
        {
            charaDir -= 120 * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.X))
        {
            charaDir += 120 * Time.deltaTime;
        }*/
        // �}�E�X�ŃJ������]
        /*float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        charaDir += mouseX;
        charaDir = Mathf.Repeat(charaDir, 360f);
        Camera.main.transform.rotation = Quaternion.Euler(0, charaDir, 0) * defaultCameraDir;*/


        //Camera.main.transform.rotation = Quaternion.Euler(0, charaDir, 0) * defaultCameraDir;


        //������̓��͂Ői��
       
        //if (Input.GetAxis("Vertical") > 0.0f)
        if (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f)
        {
       
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (input.magnitude > 1.0f) input = input.normalized;
            moveDirection.z = input.z * runForce;
            moveDirection.x = input.x * runForce;
            //�J�����̌�������ɃL�����̌�����ς���
            float Dir = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, charaDir + Dir, 0);
        }
        else
        {
            moveDirection.z = 0;
            moveDirection.x = 0;
        }

        //�d�͌v�Z
        moveDirection.y -= gravity * Time.deltaTime;

        //�ړ����s��
        //Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //�L�����̌����ɑO�i����
        Vector3 globalDirection = Quaternion.Euler(0, charaDir, 0) * moveDirection;
        controller.Move(globalDirection * Time.deltaTime);

        //�n�ʂɒ��n���Ă�����y�����ړ������Z�b�g����
        if (controller.isGrounded) moveDirection.y = 0;

        //�J�����ʒu�����݂̃L�����N�^�[�ʒu����ɐݒ肷��
        //Camera.main.transform.position = transform.position + Quaternion.Euler(0, charaDir, 0) * defaultCameraOffset;

       
    }
}