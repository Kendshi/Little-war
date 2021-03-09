using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    [SerializeField] private GameObject RocketPrefab;
    [SerializeField] private Transform Fireposition;
    private float rocketSpeed;
    [HideInInspector] public Vector3 rocketTarget { private set; get; }
    private Quaternion lookRotation;        // направление взгляда персонажа


    void Update()
    {

        lookRotation = Quaternion.LookRotation((new Vector3(RayToMouse.instance.hit.point.x, 0, RayToMouse.instance.hit.point.z)
            - new Vector3(transform.position.x, -10, transform.position.z)).normalized);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
        {
            rocketTarget = new Vector3(
                RayToMouse.instance.hit.point.x + Random.Range(-1f, 1f), 
                RayToMouse.instance.hit.point.y, 
                RayToMouse.instance.hit.point.z + Random.Range(-1f, 1f)); 
            rocketSpeed =  Vector3.Distance(transform.position, rocketTarget);
            
            Debug.Log(rocketSpeed);
            if (rocketSpeed > 15)
            {
                Shot();
            }
            else
            {
                GetComponent<AudioSource>().Play();
                Debug.Log("Выбранная цель слишком близко");   
            } 
            
        }
    }

    private void Shot()
    {
        Fireposition.transform.eulerAngles = new Vector3(Random.Range(-30f, -10f), transform.eulerAngles.y, 0f) ;
        GameObject Rocket = Instantiate(RocketPrefab,Fireposition.position, Quaternion.identity);
        Rocket.GetComponent<Rigidbody>().velocity = Fireposition.forward * rocketSpeed * Random.Range(0.8f, 2f);
        Rocket.GetComponent<RocketBehaviour>().Target = rocketTarget;
    }
}
