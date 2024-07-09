
using UnityEngine;

[ExecuteInEditMode]
public class DrawCanClimbingCollider : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        
        var boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.color = Color.green;

 
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

   
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
        }
    }
}
