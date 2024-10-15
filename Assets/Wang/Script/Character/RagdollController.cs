using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    public Animator animator;  // アニメーター
    private Rigidbody[] ragdollRigidbodies;  // ラグドールのリジッドボディ
    private Transform[] originalBoneTransforms;  // ボーンのオリジナル位置と回転を保存
    private bool isRagdoll = false;  // ラグドール状態のフラグ

    void Start()
    {
        // ラグドールリジッドボディを取得
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        // ボーンのオリジナル位置と回転を保存
        originalBoneTransforms = new Transform[ragdollRigidbodies.Length];
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            originalBoneTransforms[i] = ragdollRigidbodies[i].transform;
        }

        // ラグドール無効化（キネマティック化）
        SetRagdollActive(false);
    }

    void Update()
    {
        // ラグドール状態にする（例: スペースキーでトグル）
        if (Input.GetKeyDown(KeyCode.Space) && !isRagdoll)
        {
            ActivateRagdoll();
        }
        // ラグドールから通常アニメーションに戻す（例: Rキーでリセット）
        else if (Input.GetKeyDown(KeyCode.R) && isRagdoll)
        {
            ResetRagdoll();
        }
    }

    void ActivateRagdoll()
    {
        // アニメーターを無効化してラグドールを有効化
        animator.enabled = false;
        SetRagdollActive(true);
        isRagdoll = true;
    }

    void ResetRagdoll()
    {
        // ラグドール無効化
        SetRagdollActive(false);

        // 各ボーンをオリジナルの位置と回転にリセット
        for (int i = 0; i < ragdollRigidbodies.Length; i++)
        {
            ragdollRigidbodies[i].transform.position = originalBoneTransforms[i].position;
            ragdollRigidbodies[i].transform.rotation = originalBoneTransforms[i].rotation;
        }

        // アニメーターを再度有効化
        animator.enabled = true;
        isRagdoll = false;
    }

    void SetRagdollActive(bool isActive)
    {
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !isActive;
        }
    }
}
