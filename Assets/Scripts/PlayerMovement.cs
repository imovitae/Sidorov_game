using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    //возможсти прыжка , стоим ли на земле , контрольная точка 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;

    //стоим на земле , двигаемся 
    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        //Ссылка на контроллер перса 
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        //заземлен ли челик ?
        // Короче говоря , проверка физической сферы (является ли заземление правильным , то есть соприкасается ли у нас с полом или нет)
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //сброс скорости | если не прыгаем , то -2 , потому что скорость при прыжке увеличивается , то есть надо -2f постоянно ну чтоб не улетел челик
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //входные данные 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        //сохраняем полученные данные в векторе перемещ.
        Vector3 move = transform.right * x + transform.forward * z;   //поворот вправо крас , поворот вперед (вперед идли назад)

        //перемещ перса (формула для соответс. с частотой кадров)
        controller.Move(move * speed * Time.deltaTime);

        //проверка находися ли мы на земле => прыжок или мы в воздухе 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //падение + сила тяжести
        velocity.y += gravity * Time.deltaTime;

        //прыжок такой же как и перемещение игрока 
        controller.Move(velocity * Time.deltaTime);

        //проверка позиции (последняя с текущей) | 
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
        }
        //последнее положение фактически то где мы остановились
        lastPosition = gameObject.transform.position;
    }
}
