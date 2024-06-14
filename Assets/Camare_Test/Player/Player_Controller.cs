using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    CharacterController controller;
    Animator animator;
    public Vector3 moveDirection = Vector3.zero;
    public GameObject avatar = null; // Avatarオブジェクトへの参照
    public float gravity = 8;
    public float rotateForce = 5; //回転量
    public float runForce = 2.5f; //前進量
    public float maxRunSpeed = 2; //前進速度の制限
    public float jumpforce = 5; //ジャンプ量
    public float mouseSensitivity = 2.0f; //カメラスピード
    bool jumpableFlag = false;  //地面についているかどうか
    float jumpableCount = 0.0f; public float x = 0.2f;  //ジャンプ可能な時間 
    Vector3 defaultPosition;

    Quaternion defaultCameraDir;    //デフォルトのカメラ位置
    Vector3 defaultCameraOffset;    //デフォルトのカメラ位置補正
    float charaDir = 0;             //キャラクターの方向

    void Start()
    {
        Application.targetFrameRate = 60;
        // 必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        defaultCameraDir = Camera.main.transform.rotation;
        defaultCameraOffset = Camera.main.transform.position - transform.position;
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
       if (controller == null) return;  //キャラコントローラーが入っていない場合は終了
                                         //横方向の入力で方向転換する
                                         // transform.Rotate(0, Input.GetAxis("Horizontal") * rotateForce, 0);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = defaultPosition;
            moveDirection = new Vector3(0, 0, 0);
            Camera.main.transform.position = transform.position
                + Quaternion.Euler(0, charaDir, 0) * defaultCameraOffset;
            return;
        }

        //ジャンプ
        if (controller.isGrounded)  //地面に着地していたら
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


        //カメラ回転
        /*if(Input.GetKey(KeyCode.Z))
        {
            charaDir -= 120 * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.X))
        {
            charaDir += 120 * Time.deltaTime;
        }*/
        // マウスでカメラ回転
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        charaDir += mouseX;
        charaDir = Mathf.Repeat(charaDir, 360f);
        Camera.main.transform.rotation = Quaternion.Euler(0, charaDir, 0) * defaultCameraDir;


        //Camera.main.transform.rotation = Quaternion.Euler(0, charaDir, 0) * defaultCameraDir;


        //上方向の入力で進む
       
        //if (Input.GetAxis("Vertical") > 0.0f)
        if (Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f)
        {
       
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (input.magnitude > 1.0f) input = input.normalized;
            moveDirection.z = input.z * runForce;
            moveDirection.x = input.x * runForce;
            //カメラの向きを基準にキャラの向きを変える
            if (avatar != null)
            {
                Vector3 rotateTarget = new Vector3(moveDirection.x, 0, moveDirection.z);
                if (rotateTarget.magnitude > 0.1f)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(rotateTarget);
                    avatar.transform.rotation = Quaternion.Lerp(lookRotation, avatar.transform.rotation, 0.5f);
                }
            }
        }
        else
        {
            moveDirection.z = 0;
            moveDirection.x = 0;
        }

        //重力計算
        moveDirection.y -= gravity * Time.deltaTime;

        //移動を行う
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        //キャラの向きに前進する
        //Vector3 globalDirection = Quaternion.Euler(0, charaDir, 0) * moveDirection;
        controller.Move(globalDirection * Time.deltaTime);

        //地面に着地していたらy方向移動をリセットする
        if (controller.isGrounded) moveDirection.y = 0;

        //カメラ位置を現在のキャラクター位置を基準に設定する
       //Camera.main.transform.position = transform.position + Quaternion.Euler(0, charaDir, 0) * defaultCameraOffset;

       
    }
}