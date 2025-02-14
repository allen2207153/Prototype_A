using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Animation_Timeline : MonoBehaviour
{
    private bool _isPlayerInRange = false;               // プレイヤーが範囲内にいるかどうかを判定するフラグ
    public GameObject hintController; // ButtonHintControllerを持つGameObject
    private ButtonHintController buttonHintController; // ButtonHintControllerへの参照
    private BoxCollider hintTrigger;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayableDirector timeline; // Timelineの参照

    private void Start()
    {
        // ButtonHintControllerとBoxColliderを取得
        if (hintController != null)
        {
            hintTrigger = hintController.GetComponent<BoxCollider>();
            buttonHintController = hintController.GetComponent<ButtonHintController>();
        }
        else
        {
            Debug.LogError("HintController is not assigned.");
        }
    }

    private void OnEnable()
    {
        EventSystem.Instance.StartListening(GameEvents.Interaction, interaction_Animation);
    }

    private void OnDisable()
    {
        EventSystem.Instance.StopListening(GameEvents.Interaction, interaction_Animation);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WAAA");
            _isPlayerInRange = true; // プレイヤーが範囲内にいることをフラグで管理
            buttonHintController?.SetButtonPrompt(true); // UIを表示
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false; // プレイヤーが範囲外に出たことをフラグで管理
            buttonHintController?.SetButtonPrompt(false); // UIを非表示
        }
    }

    private void interaction_Animation()
    {
        if (_isPlayerInRange)
        {
            _animator.enabled = true;
            StartCoroutine(PlayAnimationThenTimeline());
        }
    }

    private IEnumerator PlayAnimationThenTimeline()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length); // アニメーションの完了を待機

        if (timeline != null)
        {
            timeline.Play();
            yield return new WaitForSeconds((float)timeline.duration); // Timelineの完了を待機
        }

        GetComponent<BoxCollider>().enabled = false; // 現在のトリガーを無効化
        if (hintTrigger != null)
        {
            hintTrigger.enabled = false; // Hint UI のトリガーを無効化
        }
        buttonHintController?.SetButtonPrompt(false); // UIを非表示
    }

    private void OnDrawGizmos()
    {
        var boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
