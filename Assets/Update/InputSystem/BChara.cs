using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChara : MonoBehaviour
{
    [Header("着地判定用Transformm")]
    [SerializeField] protected Transform _checkFoot;

    protected float _checkFootRadius = 0.2f;//着地判定の半径

    [Header("着地判定有効のレイヤ")]
    [SerializeField] protected LayerMask _layerMask;//着地判定有効のレイヤ

    protected enum Motion
    {
        Unnon = -1, //	無効(使えません）
        Stand,      //	停止
        Walk,       //	歩行
        Attack,     //	攻撃
        Jump,       //	ジャンプ
        Fall,       //	落下
        TakeOff,    //	飛び立つ瞬間
        Landing,    //	着地
        Jump2,      //2段ジャンプ
        Fall2,      //２段落下
    }
    protected Motion _motion;//現在のモーション
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
    /// <summary>
    /// 着地判定
    /// </summary>
    /// <returns></returns>
    protected bool CheckFoot()
    {
        return Physics.CheckSphere(
            _checkFoot.position,
            _checkFootRadius,
            _layerMask);//円形範囲を検知      
    }
}
