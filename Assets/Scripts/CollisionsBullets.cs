using UnityEngine;

public class CollisionsBullets : MonoBehaviour
{
    public void HitCollision(Collision collision, GameObject hitPrefab, float speed)
    {
        speed = 0;
        if (hitPrefab != null)
        {
            ContactPoint contact = collision.contacts[0];  // точка контакта
            GameObject hitObj = Instantiate(hitPrefab, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal));   // спаун эффекта попадания
            ParticleSystem psHit = hitObj.GetComponent<ParticleSystem>();
            if (psHit != null)
            {
                Destroy(hitObj, psHit.main.duration);
            }
            else
            {
                ParticleSystem psChild = hitObj.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitObj, psChild.main.duration);
            }
        }
        Destroy(gameObject);
    }
}
