using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChara : MonoBehaviour
{
    [Header("���n����pTransform")]
    [SerializeField]protected Transform _checkFoot;

    protected float _checkFootRadius = 0.2f;//���n����̔��a

    [Header("���n����L���̃��C��")]
    [SerializeField] protected LayerMask _layerMask;//���n����L���̃��C��

    protected enum Motion
    {
        Unnon = -1, //	����(�g���܂���j
        Stand,      //	��~
        Walk,       //	���s
        Attack,     //	�U��
        Jump,       //	�W�����v
        Fall,       //	����
        TakeOff,    //	��ї��u��
        Landing,    //	���n
        Jump2,      //2�i�W�����v
        Fall2,      //�Q�i����
    }
    protected Motion _motion;//���݂̃��[�V����
    protected Motion _preMotion;//�O��̃��[�V�������L�^����p

    protected int _moveCnt;//���݃��[�V�����ɓ���J�E���^�[
    protected int _perMoveCnt;//�O��̃��[�V�����ɓ���J�E���^�[�̍Ō�̒l

    /// <summary>
    /// ���[�V�����X�V
    /// </summary>
    /// <param name="nm_">�X�V���郂�[�V����</param>
    /// <returns></returns>
    protected bool UpdataMotion(Motion nm_)
    {
        if (nm_ == _motion)
        {
            return false;
        }
        else
        {
            //�O��̏����L�^����
            _preMotion = _motion;
            _perMoveCnt = _moveCnt;

            //���[�V�������X�V
            _motion = nm_;
            //�J�E���^�����Z�b�g
            _moveCnt = 0;
            return true;
        }
    }
    /// <summary>
    /// ���n����
    /// </summary>
    /// <returns></returns>
    protected bool CheckFoot()
    {
        return Physics.CheckSphere(
            _checkFoot.position, 
            _checkFootRadius, 
            _layerMask);//�~�`�͈͂����m      
    }
}
