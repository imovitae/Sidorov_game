using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienChaseState : StateMachineBehaviour
{
    // Компонент NavMeshAgent для навигации
    UnityEngine.AI.NavMeshAgent agent;
    Transform player;
    // Скорость преследования
    public float chaseSpeed = 6f;
    // Расстояние прекращения преследования + расстояние для атаки
    public float stopChasingDistance = 21f;
    public float attackingDistance = 4f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Инициализация ссылки на игрока и агента
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Установка цели для преследования (позиция игрока) и поворот к игроку
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        // Определение расстояния до игрока | Если дальше чем stopChasingDistance, прекратить преследование
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        // Если дальше чем stopChasingDistance, прекратить преследование
        if (distanceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        // Если ближе чем attackingDistance, начать атаку
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Остановка движения агента
        agent.SetDestination(animator.transform.position);
    }
}
