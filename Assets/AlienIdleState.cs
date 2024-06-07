using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//логика для "штатов" | переход пришельца в разные состояния
public class AlienIdleState : StateMachineBehaviour
{
    //сообщаем пришельцу в какое состояние он должен перейти (патрулирование) + перемещение по кругу , а затем счетчик для режима ожиания | пока что таймер ставим и тд 
    float timer;
    public float idleTime = 0f;

    Transform player;

    //радиус зоны обнаружения 
    public float detectionAreaRadius = 18f;

    //запуск метода , когда мы входим в режим ожидания 
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        //ссылка на плеер
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    //пока мы находимся в режиме ожидания 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //тут мы хотим увеличивать таймер каждые две сек и проверять достиг ли таймер времени простоя и если достигло значит мы должны перейти в режим патруля 
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isPatroling", true);
        }
        //действительно ли игрок обнаружен? если да , то переходим в режим преследования 
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionAreaRadius)//провекра на то что входит ли игрок в зону обнаружения
        {
            animator.SetBool("isChasing", true);
        }
    }
  
}
