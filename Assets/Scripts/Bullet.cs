using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //фиксация сталкновения с объектом 
        if (collision.gameObject.CompareTag("Target"))
        {
            //Попала в объект?! Уничтожай пулю | Если не попала то пуля пропадет через 3 сек
            print("hit"+collision.gameObject.name+ "!");
            Destroy(gameObject);  
        }
    }

}
