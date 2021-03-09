using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMaster : MonoBehaviour
{
    public static SpawnMaster singlton;

    public bool timerStart = false;                             //включает/выключает таймер обратного отсчета до возрождения

    private float timer = 5;                                    //время до возрождения

    [SerializeField] private Transform[] startPoint;            //Точки в которых возрождаются враги
    [SerializeField] private GameObject enemyPrefab;            //Префаб врага
    [SerializeField] private GameObject playerOnScene;          //объект игрока
    [SerializeField] private GameObject playerPrefab;           //Префаб игрока
    [SerializeField] private Transform playerStartPosition;     //Начальная позиция игрока
    [SerializeField] private Text timerText;                    //Текст таймера возрождения на экране

    private void Awake()
    {
        singlton = this;
    }

    void Start()
    {//Создаем 3-х врагов
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyPrefab, startPoint[i].position, Quaternion.identity);
        }
    }

    void Update()
    {//Таймер отсчета времени возрождения игрока
        if (timerStart) //переменная включается в скрипте игрока, после смерти
        {
            if (Mathf.Round(timer) != 0)
            {
                timerText.enabled = true; //делаем текст видимым
                timer -= Time.deltaTime;  // ведем отсчет
                timerText.text = "Время до возрождения: " + Mathf.Round(timer).ToString(); // выводим информацию на экран

            }
            else if (Mathf.Round(timer) == 0)
            {
                timer = 5;                  //Возвращаем таймер в исходное состояние
                timerText.enabled = false;  //делаем текст невидимым
                timerStart = false;         //выключаем операцию
            }
        }

        if (playerOnScene == null)
        {//Если персонаж игрока умер, создаем нового
            playerOnScene = Instantiate(playerPrefab, playerStartPosition.position, Quaternion.identity);
        }
    }

    public IEnumerator SpawnEnemy()
    {//Через 5 секунд после смерти врага, создаем нового в случайно точке
        yield return new WaitForSeconds(5);
        Instantiate(enemyPrefab, startPoint[Random.Range(0, 2)].position, Quaternion.identity);
    }
}
