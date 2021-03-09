using UnityEngine;

// класс для генерации снарядов
public class Firing : MonoBehaviour
{
    public GameObject projectilePrefab;         // префаб снаряда
    [SerializeField] Transform firePosition;

    private Animator animator;
    private AudioSource gunFire;                //звук выстрела

    void Start()
    {
        animator = GetComponent<Animator>();
        gunFire = GetComponent<AudioSource>();  // Получаем аудио источник со звуком выстрела
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetTrigger("StopFire");
        }
    }

    public void Fire()
    {
        Instantiate(projectilePrefab, firePosition.position, transform.rotation);  // спаун снаряда
        gunFire.Play();
    }
}
