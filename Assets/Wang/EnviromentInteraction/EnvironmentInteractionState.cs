using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 環境とのインタラクションに関連する状態の抽象クラス
public abstract class EnvironmentInteractionState : BaseState<EnvironmentInteractionStateMachine.EEnvironmentInteractionState>
{

    protected EnvironmentInteractionContext Context;
    private float _movingAwayOffset = .05f;
    bool _shouldReset;

    // コンストラクタ：コンテキストと状態キーを受け取って初期化
    public EnvironmentInteractionState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState stateKey) : base(stateKey)
    {
        Context = context;
    }

    // 指定の位置とコライダーの最近接点を取得
    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck)
    {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }

    // IKターゲット位置の追跡を開始するメソッド
    protected void StartIkTargetPositionTracking(Collider intersectingCollider)
    {
        // 追跡していない場合にのみ処理を開始
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && Context.CurrentIntersectingCollider == null)
        {
           
            Context.CurrentIntersectingCollider = intersectingCollider;

            // ルートTransformの位置からの最近接点を取得し、左右どちらの肩が近いかを設定
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);

            // IKターゲット位置を設定
            SetIkTargetPosition();
        }
    }

    // IKターゲット位置を更新するメソッド
    protected void UpdateIKTargetPosition(Collider intersectingCollider)
    {
        // 現在の接触中のコライダーと一致する場合のみIKターゲット位置を更新
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            SetIkTargetPosition();
        }
    }

    // IKターゲット位置の追跡をリセットするメソッド
    protected void ResetIkTargetPositionTracking(Collider intersectingCollider)
    {
        // 現在の接触中のコライダーと一致する場合、追跡をリセット
        if (intersectingCollider == Context.CurrentIntersectingCollider)
        {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        }
    }

    // IKターゲット位置を設定する内部メソッド
    private void SetIkTargetPosition()
    {
        // 肩の高さでのコライダー上の最も近い点を取得し、それを基にIKターゲット位置を設定
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(
            Context.CurrentIntersectingCollider,
            new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z)
        );

        // 肩からコライダーへの方向を取得し、正規化してオフセットを計算
        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistance = .05f; // オフセット距離
        Vector3 offset = normalizedRayDirection * offsetDistance;

        // 最近接点にオフセットを加算し、IKターゲットTransformに適用
        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIkTargetTransform.position = offsetPosition;
    }
}