using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPatrolingState : StateMachineBehaviour
{
    //для проверки патрулирования 
    float timer;
    public float patrolingTime = 10f;

    //ссылки
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;

    //зона обнаруж и скорость патр
    public float detectionArea = 18f;
    public float patrolSpeed = 2f;

    //список трансформации (от одной точки к другой при путрулировании например ) 
    List<Transform> waypointsList = new List<Transform>();
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //инициализация игрока и агента 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //скорость агента = скорости патр
        agent.speed = patrolSpeed;
        timer = 0;
        //перемещ .  в первую точку маршрута 
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints");//ссылка на кластер путевых точек 
        foreach(Transform t  in waypointCluster.transform)//перебор всех дочерних элементов + добавляем их в путь.точку 
        {
            waypointsList.Add(t);
        }
        //ищем случайную путь.точку в списке | создаем случайное число от 0 до кол-ва путь.точек , находим случайное число и возвращаем его , затем получаем эту позицию и устанавливаем ее в качестве пункта назнач. агента 
        Vector3 nextPosition= waypointsList[Random.Range(0, waypointsList.Count)].position;
        agent.SetDestination(nextPosition);
    }
    //проверка точки маршрута и если достиг он переместится в др точку марш
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);//внутри заданного пунтка назнач уставноим случайную точку марш
        }
        //проверка времени заправки | если заончилось вернуться в режим ожидания 
        timer += Time.deltaTime;
        if(timer > patrolingTime)
        {
            animator.SetBool("isPatroling", false);

        }

        //проверка | находится ли игрок в зоне достимгаемости , чтобы могли начать преследовать его 
        //действительно ли игрок обнаружен? если да , то переходим в режим преследования 
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)//провекра на то что входит ли игрок в зону обнаружения
        {
            animator.SetBool("isChasing", true);
        }

    }
    //остановка агента когда он выйдет из данного состояния 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);//установка как конечное
    }
}
