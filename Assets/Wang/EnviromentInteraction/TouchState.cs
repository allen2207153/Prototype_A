using UnityEngine;

// "Touch" 状態を表すクラス。キャラクターがオブジェクトに接触している際のインタラクションを管理
public class TouchState : EnvironmentInteractionState
{
    // コンストラクタ：コンテキストと状態キーを受け取って初期化
    public TouchState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        // 冗長な再定義です。削除しても影響はありません。
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

    // トリガーに入ったときに呼ばれるメソッド（現在は空）
    public override void OnTriggerEnter(Collider other) { }

    // トリガー内にいる間に呼ばれるメソッド（現在は空）
    public override void OnTriggerStay(Collider other) { }

    // トリガーから出たときに呼ばれるメソッド（現在は空）
    public override void OnTriggerExit(Collider other) { }
}