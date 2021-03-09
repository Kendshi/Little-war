using UnityEngine;

public class RocketLaunch : MonoBehaviour
{
    [SerializeField] private Transform Spawn;   //точка в которой происходит появление ракет
    [SerializeField] private GameObject Bullet; //Префаб ракеты

    private Transform selfTransform;        //совственный трасформ объекта
    private Vector3 Target;                 //Координаты в которые должна лететь ракета
    private float AngleInDegrees;           //угол наклона 
    private Quaternion lookRotation;        //направление стрельбы ракетницей
    private float dist;                     //расстояниме от игрока до цели
    private CharacterControl character;     //Скрипт управления персонажем игрока

    void Start()
    {
        selfTransform = transform;          //Кешируем трансформ
        character = GetComponentInParent<CharacterControl>();   //Получаем доступ к скрипту персонажа игрока
    }

    void Update()
    {
        lookRotation = Quaternion.LookRotation((new Vector3(RayToMouse.instance.hit.point.x, 0, RayToMouse.instance.hit.point.z)
            - new Vector3(selfTransform.position.x, -10, selfTransform.position.z)).normalized); //высчитываем поворот ракетницы, чтобы она смотрела в сторону мышки

        selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, lookRotation, 5f * Time.deltaTime); //поворачиваем ракетницу плавно, вслед за мышкой

        if (character == null)
        {
            character = GetComponentInParent<CharacterControl>();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!character.death)
            {
                Target = RayToMouse.instance.hit.point;     //Задаем целью точку на сцене, на которую кликнули мышкой
                Shot2();
            }                                  
        }
    }

    /// <summary>
    /// Создание ракеты и расчет стартовых условий для запуска
    /// </summary>
    private void Shot2()
    {
        dist = Vector3.Distance(selfTransform.position, Target);     //Дистанция между игроком и целью
        Vector3 pos = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), 0f);    //случайное значение вектора который мы прибавляем к стартовому положению ракеты
        AngleInDegrees = (dist - Random.Range(6, 16)) * -1;          //расчитываем какой должен быть угол наклона пусковой точки чтобы попасть в цель
                                                                     //для рассчета, за основу берем дистанцию до цели и отнимаем случайное значение для создания "разброса" снарядов
        Spawn.eulerAngles = new Vector3(AngleInDegrees, Spawn.eulerAngles.y, 0f);   //задаем угол точке запуска снарядов
        GameObject newBullet = Instantiate(Bullet, Spawn.position +pos, Quaternion.identity); //создаем ракету, к позиции появления прибавляем случайное значение для увеличения "разброса"
        newBullet.GetComponent<Rigidbody>().velocity = Spawn.forward * 20;  //Задаем скорость движения снаряда 
    }

    #region другой вариант реализации стрельбы
    //private void Shot()
    //{
    //    AngleInDegrees = Random.Range(-30f, -10f);

    //    Vector3 fromTo = Target - selfTransform.position;
    //    Vector3 fromToXYZ = new Vector3(fromTo.x, 0f, fromTo.z);

    //    float x = fromToXYZ.magnitude;
    //    float y = fromTo.y;
    //    float AnglesInRadians = AngleInDegrees * Mathf.PI / 180;

    //    float v2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(AnglesInRadians) * x) * Mathf.Pow(Mathf.Cos(AnglesInRadians), 2));
    //    float v = Mathf.Sqrt(Mathf.Abs(v2));

    //    Spawn.eulerAngles = new Vector3(AngleInDegrees, Spawn.eulerAngles.y /*+ Random.Range(-5f, 5f)*/, 0f);
    //    GameObject newBullet = Instantiate(Bullet, Spawn.position, Quaternion.identity);
    //    newBullet.GetComponent<Rigidbody>().velocity = Spawn.forward * v;
    //}
    #endregion
}
