using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{//Скрипт ограничивает время в течение которого можно получить урон от взрыва
    private Collider coll;

    void Start()
    {
        coll = GetComponent<Collider>();
        Invoke(nameof(CloseCollider), 1f);
    }

    private void CloseCollider()
    {
        coll.enabled = false;
    }
    
}
