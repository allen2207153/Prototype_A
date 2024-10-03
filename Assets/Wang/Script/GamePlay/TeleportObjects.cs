using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObjects : MonoBehaviour
{
    // 主要キャラクター
    public GameObject primaryCharacter;

    // サブキャラクター
    public GameObject secondaryCharacter;

    // 主要キャラクターの移動先
    public Transform primaryTargetPosition;

    // サブキャラクターの移動先
    public Transform secondaryTargetPosition;

    // トリガーに衝突したときに呼ばれるメソッド
    private void OnTriggerEnter(Collider other)
    {
        // もし主要キャラクターがトリガーに入ったら
        if (other.gameObject == primaryCharacter)
        {
            // 主要キャラクターをテレポート
            TeleportObject(primaryCharacter, primaryTargetPosition);

            // サブキャラクターをテレポート
            TeleportObject(secondaryCharacter, secondaryTargetPosition);
        }
    }

    // キャラクターを指定された位置にテレポートする
    void TeleportObject(GameObject character, Transform targetPosition)
    {
        CharacterController controller = character.GetComponent<CharacterController>();

        // CharacterControllerがある場合は一時的に無効化してから位置を変更
        if (controller != null)
        {
            controller.enabled = false; // 無効化
            character.transform.position = targetPosition.position; // テレポート
            controller.enabled = true;  // 再度有効化
        }
        else
        {
            // CharacterControllerがない場合は通常の移動処理
            character.transform.position = targetPosition.position;
        }
    }
}
