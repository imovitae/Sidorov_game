using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;
    private void OnCollisionEnter(Collision objectWeHit)
    {
        //фиксация сталкновения с объектом 
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            //Попала в объект?! Уничтожай пулю | Если не попала то пуля пропадет через 3 сек
            print("hit"+ objectWeHit.gameObject.name+ "!");
            CreateBulletImpactEffect(objectWeHit);//точное положение куда мы попали 
            Destroy(gameObject);  
        }  
        
        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            //Попала в объект?! Уничтожай пулю | Если не попала то пуля пропадет через 3 сек
            print("hit a wall ");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);  
        }

        if (objectWeHit.gameObject.CompareTag("Alien"))
        {
            objectWeHit.gameObject.GetComponent<Alien>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }


    void CreateBulletImpactEffect(Collision objectWeHit )//поражаем цель +эффект
    {
        //точка соприкосновения 
        ContactPoint contact= objectWeHit.contacts[0];  //первая точка куда попадет пуля
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,//позиция создания
            Quaternion.LookRotation(contact.normal)//вращение взгялад + достижение нормальной цели(чего мы хотим)
            
            );//фактического отверстия

        hole.transform.SetParent(objectWeHit.gameObject.transform);    //дыра - дочерний элемент внутри родительского | трансформация объектов в которые поопали 

    }
}
