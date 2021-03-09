using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Tooltip("скорость перемещения персонажа")]
    [SerializeField] private float Speed = 5f;          // скорость перемещения персонажа

    private RagdollOn ragdoll;
    private Rigidbody body;                 // физическое тело игрока
    private Vector3 inputs;                 // вектор движения получаемый от нажатий кнопок управления игроком
    private Transform playerTransform;      // трансформ нашего персонажа
    private Vector3 moveGlobal;             // вектор перемещения
    private Quaternion lookRotation;        // направление взгляда персонажа
    private Animator animator;              // аниматор
    public bool death = false;              //переключатель состояния жизни/смерти
    private bool OnOff = true;              //вспомогательный одноразовый выключатель


    void Start()
    {
        inputs = Vector3.zero;                          // обнуление ввода перед началом игры на всякий случай
        body = GetComponent<Rigidbody>();               // получение компонента Rigidbody
        playerTransform = GetComponent<Transform>();    // получение Трансформа персонажа
        animator = GetComponent<Animator>();            // получение Аниматора
        ragdoll = GetComponent<RagdollOn>();            // Получаем компонент Ragdoll
    }

    void Update()
    {
        inputs.x = Input.GetAxis("Horizontal"); // получаем от игрока вектор перемещения по x кнопки A и D
        inputs.z = Input.GetAxis("Vertical");   // получаем от игрока вектор перемещения по y кнопки W и S
        
        lookRotation = Quaternion.LookRotation((new Vector3(RayToMouse.instance.hit.point.x, 0, RayToMouse.instance.hit.point.z) 
            - new Vector3(playerTransform.position.x, 0, playerTransform.position.z)).normalized);

        moveGlobal = Quaternion.AngleAxis(RayToMouse.instance.cameraYRotation, Vector3.up) * inputs;    // перемещение персонажа
        Vector3 moveLocal = playerTransform.InverseTransformDirection(moveGlobal);  // преобразование глобальных координат перемещения персонажа в локальные
        animator.SetFloat("Forward", moveLocal.z);                                  // анимация движения вперёд / назад
        animator.SetFloat("Right", moveLocal.x);                                    // анимация движения влево / вправо

        if (death)
        {//В случае смерти запускаем таймер обратного отчета в SpawnMaster
            if (OnOff)
            {
                OnOff = false;
                SpawnMaster.singlton.timerStart = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!death)
        {
            body.MovePosition(body.position + moveGlobal * Speed * Time.fixedDeltaTime); //перемещение персонажа
            if (!Input.GetMouseButton(2))
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, 5f * Time.fixedDeltaTime); // Поворот персонажа
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Смерть");
            death = true;
            ragdoll.DoRagdoll(true);
            Destroy(gameObject, 5);
        }
    }
}
