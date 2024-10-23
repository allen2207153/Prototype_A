using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChara : MonoBehaviour
{

    //頭上
    //追加時間：20240713＿八子遥輝
    [Header("頭上判定用Transformm")]
    [SerializeField] protected Transform _checkHead;
    protected float _checkHeadRadius = 0.2f;//頭上判定の半径

    //追加時間：20240713＿八子遥輝
    [Header("頭上判定有効のレイヤ")]
    [SerializeField] protected LayerMask _layerMaskHead;//頭上判定有効のレイヤ

    //足元
    [Header("着地判定用Transformm")]
    [SerializeField] protected Transform _checkFoot;

    protected float _checkFootRadius = 0.15f;//着地判定の半径

    [Header("着地判定有効のレイヤ")]
    [SerializeField] protected LayerMask _layerMask;//着地判定有効のレイヤ

    /// <summary>
    /// GetMotion()メソッドを使いください
    /// </summary>
    public enum Motion
    {
        Unnon = -1,   //	無効(使えません）
        Stand,        //	停止
        Walk,         //	歩行
        Attack,       //	攻撃
        Jump,         //	ジャンプ
        Fall,         //	落下
        TakeOff,      //	飛び立つ瞬間
        Landing,      //	着地
        Jump2,        //　　2段ジャンプ
        Fall2,        //　　2段落下
        JumpToHangingTakeOff,
        JumpToHanging,     //ぶら下がる前のジャンプ（Jumpとは違う動き方）
        Hanging_ByJump,    //ぶら下がる
        Hanging_ByCollider,//コライダーによるぶら下がる
        ClimbingUp,     //登り
        Crouching_Enter,//しゃがみながら入る
        Crouching_Idle, //しゃがみ待機
        Crouching_Walk, //しゃがみ歩き
        Crouching_Exit  //しゃがみながら出る

    }
    protected Motion _motion = Motion.Fall;//現在のモーション
    protected Motion _preMotion;//前回のモーションを記録する用

    protected int _moveCnt;//現在モーションに入るカウンター
    protected int _perMoveCnt;//前回のモーションに入るカウンターの最後の値

    /// <summary>
    /// モーション更新
    /// </summary>
    /// <param name="nm_">更新するモーション</param>
    /// <returns></returns>
    protected bool UpdataMotion(Motion nm_)
    {
        if (nm_ == _motion)
        {
            return false;
        }
        else
        {
            //前回の情報を記録する
            _preMotion = _motion;
            _perMoveCnt = _moveCnt;

            //モーションを更新
            _motion = nm_;
            Debug.Log(_motion.ToString());
            //カウンタをリセット
            _moveCnt = 0;
            return true;
        }
    }


    //追加時間：20240713＿八子遥輝
    /// <summary>
    /// 頭上判定
    /// </summary>
    /// <returns></returns>
    protected bool CheckHead()
    {
        return Physics.CheckSphere(
            _checkHead.position,
            _checkHeadRadius,
            _layerMaskHead);//円形範囲を検知
    }



    /// <summary>
    /// 着地判定
    /// </summary>
    /// <returns></returns>
    protected bool CheckFoot()
    {
        // 使用球形檢測來判斷地面
        bool isOnGroundSphere = Physics.CheckSphere(_checkFoot.position, _checkFootRadius, _layerMask);

        // 使用Raycast來進一步確認是否在斜坡或階梯上
        bool isOnGroundRaycast = Physics.Raycast(_checkFoot.position, Vector3.down, _checkFootRadius + 0.1f, _layerMask);

        // 如果任一檢測成功，則認為角色在地面上
        return isOnGroundSphere || isOnGroundRaycast;
    }


    //更新_追加時間：20240713＿八子遥輝
    /// <summary>
    /// 頭上と着地判定の範囲を描く
    /// </summary>
    protected void OnDrawGizmos()
    {
        //追加時間：20240713＿八子遥輝
        //黄色に設定
        Gizmos.color = Color.yellow;
        //頭上判定の範囲を描く
        Gizmos.DrawWireSphere(_checkHead.position, _checkHeadRadius);

        //黄色に設定
        Gizmos.color = Color.yellow;
        //着地判定の範囲を描く
        Gizmos.DrawWireSphere(_checkFoot.position, _checkFootRadius);
    }
    //public関数------------------------------------------------
    public Motion GetMotion()
    {
        return _motion;
    }
}
