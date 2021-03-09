using UnityEngine;

public class RocketBehaviour : CollisionsBullets
{
    [SerializeField] private GameObject hitExplosionParticle;   //Префаб эффекта взрыва
    private Rigidbody body;
    public Vector3 Target;      //Цель которую нужно поразить
    private bool On = false;    //вспомогательная переменная запускает прямолинейное движение к цели
    private Transform selfTransform;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody>();   //Кешируем Rigidbody
        selfTransform = transform;          //Кешируем Трансформ
        Invoke(nameof(Explode), 2f);        //Если прошло 2 секунды, то вызываем метод взрыва ракеты с удалением объекта
    }
  
    private void FixedUpdate()
    { //двигаем текущий объект к цели, прямолинейно
        if (On)
            selfTransform.position = Vector3.MoveTowards(selfTransform.position, Target, 15 * Time.fixedDeltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {//Если тригерный коллайдер замечает врага, то назначает его координаты целью ракеты и запускает движение к нему
            Target = other.transform.position;
            On = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {//Если мы с чем-то сталкиваемся, то запускаем метод взрыва ракеты
        float speed = 0.1f;
        HitCollision(collision, hitExplosionParticle, speed);
    }

    /// <summary>
    /// Взрыв ракеты
    /// </summary>
    private void Explode()
    {
        GameObject explosion = Instantiate(hitExplosionParticle, selfTransform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}
