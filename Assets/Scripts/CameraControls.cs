using UnityEngine;
using Cinemachine;
//Позволяет двигать камеру по оси Х 
public class CameraControls : MonoBehaviour
{
    private CinemachineFreeLook vcam;
    [SerializeField] private Transform playerTrans;
    public CharacterControl character;
   
    void Start()
    {
        character = playerTrans.gameObject.GetComponent<CharacterControl>();
        vcam = GetComponent<CinemachineFreeLook>();
    }

   
    void Update()
    {

        if (playerTrans == null)
        {//Если персонаж игрока удалился со сцены и появился новый, то привязываем камеру к нему
                playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                vcam.m_Follow = playerTrans;
                vcam.m_LookAt = playerTrans;
                character = playerTrans.gameObject.GetComponent<CharacterControl>();
        }

        if (character.death)
        {
            vcam.m_LookAt = null;
        }

        if (Input.GetKey(KeyCode.Mouse2))
            vcam.m_XAxis.m_InputAxisName = "Mouse X";
        else
        {
            vcam.m_XAxis.m_InputAxisName = "";
            vcam.m_XAxis.m_InputAxisValue = 0;
        }
    }
}
