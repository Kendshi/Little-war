using UnityEngine;

public class RayToMouse : MonoBehaviour
{
    public static RayToMouse instance;                          //синглтон

    private Camera mainCamera;                                  // основная камера
    private Transform mainCameraTransform;                      // transform основной камеры
    [HideInInspector] public RaycastHit hit;                    
    [HideInInspector] public float cameraYRotation { private set; get; } // угол поворота камеры вокруг оси Y 

    private void Awake()
    {
        instance = this;        
    }

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();        // находим на сцене главную камеру
        mainCameraTransform = mainCamera.transform;     // получение Трансформа главной камеры для оптимизации
    }

    void Update()
    {
        cameraYRotation = mainCameraTransform.eulerAngles.y;
        Ray();
    }

    /// <summary>
    /// Создает луч из из камеры к курсору
    /// </summary>
    public void Ray()
    { 
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);   // создаем луч который выходит из камеры и стремится к позиции курсора
        Physics.Raycast(cameraRay, out hit);                                // получаем луч
    }
}
