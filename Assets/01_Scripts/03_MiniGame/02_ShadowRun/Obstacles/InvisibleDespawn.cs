using UnityEngine;

public class InvisibleDespawn : MonoBehaviour
{
    [SerializeField] LayerMask _layer;

    private void OnTriggerEnter(Collider other)
    {
        if ((_layer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent<InvisibleController>(out var invisible))
            {
                Logger.Log("투명 인간 삭제");
                Destroy(invisible.gameObject);
            }
        }
    }
}
