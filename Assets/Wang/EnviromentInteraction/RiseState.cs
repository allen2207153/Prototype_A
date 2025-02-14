using UnityEngine;

// "Rise" 状態を表すクラス。環境とのインタラクションでキャラクターが上昇する際の状態
public class RiseState : EnvironmentInteractionState
{
    // コンストラクタ：コンテキストと状態キーを受け取って初期化
    public RiseState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState estate) : base(context, estate)
    {
        // ローカル変数として再定義している "Context" は冗長です。削除可能です。
        EnvironmentInteractionContext Context = context;
    }

    // 状態開始時に呼ばれるメソッド
    public override void EnterState() { }

    // 状態終了時に呼ばれるメソッド
    public override void ExitState() { }

    // 毎フレーム更新されるメソッド
    public override void UpdateState() { }

    // 次の状態を取得するメソッド。ここでは StateKey を返します
    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        return StateKey;
    }

    // トリガーに入ったときに呼ばれるメソッド
    public override void OnTriggerEnter(Collider other) { }

    // トリガー内にいる間に呼ばれるメソッド
    public override void OnTriggerStay(Collider other) { }

    // トリガーから出たときに呼ばれるメソッド
    public override void OnTriggerExit(Collider other) { }
}