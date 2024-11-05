using UnityEngine;

// ApproachState クラスは EnvironmentInteractionState から継承し、特定の環境とのインタラクション状態を表現する
public class ApproachState : EnvironmentInteractionState
{
    // コンストラクタでコンテキストと状態キーを設定
    public ApproachState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        // EnvironmentInteractionContext のローカル参照（
        EnvironmentInteractionContext Context = context;
    }

    // 状態が開始されたときの処理
    public override void EnterState() { }

    // 状態が終了したときの処理
    public override void ExitState() { }

    // 毎フレーム呼び出される状態更新処理
    public override void UpdateState() { }

    // 次の状態を返す処理（StateKey はこの状態が保持するキーを返す）
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    // コライダーがトリガーに入ったときの処理
    public override void OnTriggerEnter(Collider other) { }

    // コライダーがトリガー内にとどまっているときの処理
    public override void OnTriggerStay(Collider other) { }

    // コライダーがトリガーから出たときの処理
    public override void OnTriggerExit(Collider other) { }
}