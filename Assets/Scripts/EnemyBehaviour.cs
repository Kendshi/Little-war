using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private RagdollOn ragdoll;
    private Animator animator;
    private Transform target;                           //цель к которой стремится персонаж
    private Transform selfTransform;                    //Собственный трансформ
    private bool death = false;                         //переключатель состояния жизни/смерти
    private bool OnOff = true;                          //вспомогательный одноразовый выключатель
    private float distance;                             //Расстояние от персонажа до цели
    private AudioSource gunFire;

    [SerializeField] private GameObject bulletPrefab;   //Префаб пули
    [SerializeField] private Transform firePoint;       //ТОчка откуда ведется стрельба
    [SerializeField] private float distanceToAttack;    //Расстояние до цели с которого персонаж начнет стрельбу
    

    void Start()
    {
        ragdoll = GetComponent<RagdollOn>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        selfTransform = GetComponent<Transform>();
        gunFire = GetComponent<AudioSource>();
        FindPlayer();   
    }

    /// <summary>
    /// Ищет игрока на сцене и делает его целью
    /// </summary>
    private void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    void Update()
    {
        if (target != null)
        {// высчитываем расстояние до цели
            distance = Vector3.Distance(target.position, selfTransform.position);
        }
        
        if (!death)
        {//Если еще не достигли цели, то включаем анимацию бега
            if (!HasReached())
                animator.SetBool("Run", true);
            else 
                animator.SetBool("Run", false);
        }
        
        if (target != null && !death)
        {//Задаем позицию цели как место назначения
            agent.SetDestination(target.position);
        }

        if (distance < distanceToAttack)
        {//Если достигли расстояния для атаки то смотрим на персонажа и начинаем стерлять
            FaceTarget();
            animator.SetBool("Attack", true);
        }
        else
            animator.SetBool("Attack", false);

        if (target == null)
        {//Если игрок умер и его персонаж возродился снова находим его в качестве цели
            FindPlayer();
        }

        if (death)
        {//Если умерли, создаем еще одного врага
            if (OnOff)
            {
                OnOff = false;
                SpawnMaster.singlton.StartCoroutine(SpawnMaster.singlton.SpawnEnemy());
            }
        }
    }

    /// <summary>
    /// Метод стрельбы, используется в качестве ивента в анимации
    /// </summary>
    public void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, selfTransform.rotation);
        gunFire.Play();
    }

    /// <summary>
    /// Определяет достиг ли персонаж места назначения
    /// </summary>
    /// <returns></returns>
    private bool HasReached()
    {
         if (agent.remainingDistance < distanceToAttack)
             return true;
         else
             return false;
    }

    /// <summary>
    /// Выравнивает угол разворота персонажа, чтобы он смотрел на цель
    /// </summary>
    private void FaceTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - selfTransform.position).normalized;
            Quaternion lookRotatin = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, lookRotatin, Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Death();
        }
    }

    private void Death()
    {
        death = true;
        agent.enabled = false;
        ragdoll.DoRagdoll(true);
        Destroy(gameObject, 5);
    }
}
