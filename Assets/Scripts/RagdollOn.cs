using UnityEngine;

public class RagdollOn : MonoBehaviour
{
    private Animator animator;              // Аниматор персонажа
    private Collider[] AllColider;          // Коллайдеры Ragdoll
    private Rigidbody[] Allrigidbodies;     // физические тела Ragdoll
    private Collider mainCol;               // основной коллайдер
    private Rigidbody body;                 // физическое тело игрока
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        mainCol = GetComponent<Collider>();                     //Получаем основной коллайдер
        AllColider = GetComponentsInChildren<Collider>(true);   //Получаем все коллайдеры на персонаже
        Allrigidbodies = GetComponentsInChildren<Rigidbody>();  //Получаем все Rigidbody на персонаже
        DoRagdoll(false);                                       //Выключаем Ragdoll
    }

    /// <summary>
    /// Включает/выключает Ragdoll поведение персонажа
    /// </summary>
    /// <param name="IsRagdoll"></param>
    public void DoRagdoll(bool IsRagdoll)
    {
        animator.enabled = !IsRagdoll;

        foreach (var col in AllColider)
        {
            col.enabled = IsRagdoll;
        }

        foreach (var rb in Allrigidbodies)
        {
            rb.isKinematic = !IsRagdoll;
        }

        mainCol.enabled = !IsRagdoll;
        body.isKinematic = IsRagdoll;
    }
}
