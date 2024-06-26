using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAttackState : StateMachineBehaviour
{
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;
    //если расстояние больше чем надо то атака прекращается , в общем поплавок такой 
    public float stopAttackingDistance = 4f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //инициализация игрока и агента 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //способ смотреть на игрока даже когда он атакует его 
        LookAtPlayer();
        //должно ли прекратить атаку
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0,yRotation,0);
    }
}
