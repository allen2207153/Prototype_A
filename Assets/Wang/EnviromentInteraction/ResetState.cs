using UnityEngine;

// リセット状態のクラス、環境インタラクションをリセットする役割
public class ResetState : EnvironmentInteractionState
{
    // コンストラクタ：コンテキストと状態キーを受け取って初期化
    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        // ローカル変数として Context を再定義していますが、コンストラクタの引数 context に格納するだけなので不要です
        EnvironmentInteractionContext Context = context;
    }

    // 状態開始時に呼ばれるメソッド
    public override void EnterState() { }

    // 状態終了時に呼ばれるメソッド
    public override void ExitState() { }

    // 毎フレーム更新されるメソッド
    public override void UpdateState() { }

    // 次の状態を取得するメソッド
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
    
        return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
    }

    // トリガーに入ったときに呼ばれるメソッド
    public override void OnTriggerEnter(Collider other) { }

    // トリガー内にいる間に呼ばれるメソッド
    public override void OnTriggerStay(Collider other) { }

    // トリガーから出たときに呼ばれるメソッド
    public override void OnTriggerExit(Collider other) { }
}