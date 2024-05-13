using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500f;
    float xRotation = 0f;
    float yRotation = 0f;


    //степени зажима 

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    // Start is called before the first frame update
    void Start()
    {
        //Блокировка курсора 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //входные данные для мышки 
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;//перемещение по оси X 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;//перемещение по оси Y 

        //Вращение вокрус оси (Смотря вверх и вниз)
        xRotation -= mouseY;

        //Фиксация вращения(блокировка при слишком поднятой камере) 
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Вращение вокрус оси (Смотря влево и вправо)
        yRotation += mouseX;

        //Следование тела за поворотом 
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
