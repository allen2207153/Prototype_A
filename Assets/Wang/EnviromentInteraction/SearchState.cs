using UnityEngine;

// "Search" 状態を表すクラス。キャラクターが環境を探索する際のインタラクションを実装
public class SearchState : EnvironmentInteractionState
{
    // コンストラクタ：コンテキストと状態キーを受け取って初期化
    public SearchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        // ローカル変数として "Context" を再定義しているのは冗長です。削除可能です。
        EnvironmentInteractionContext Context = context;
    }

    // 状態が開始されたときに呼ばれるメソッド（現在は空）
    public override void EnterState() { }

    // 状態が終了したときに呼ばれるメソッド（現在は空）
    public override void ExitState() { }

    // 毎フレーム更新されるメソッド（現在は空）
    public override void UpdateState() { }

    // 次の状態を取得するメソッド
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    // トリガーに入ったときに IK ターゲット位置の追跡を開始
    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetPositionTracking(other);
    }

    // トリガー内にいる間に IK ターゲット位置を更新
    public override void OnTriggerStay(Collider other)
    {
        UpdateIKTargetPosition(other);
    }

    // トリガーから出たときに IK ターゲット位置の追跡をリセット
    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }
}