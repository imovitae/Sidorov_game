using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent navAgent;
    // Инициализация компонентов
    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent= GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Alien game object.");
            // Если аниматора нет, то можно деактивировать объект для предотвращения ошибок
            // gameObject.SetActive(false);
        }
    }

    // Метод для получения урона и изменения HP
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("DIE");
                
            }
           
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("DAMAGE");
            }
        } 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);//Атака//стоп 

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 18f);//заметил , преследуй

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 21f);//прекратить преслед
    }
}
