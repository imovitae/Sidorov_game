
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set; }//тип экземпляра 

    public GameObject bulletImpactEffectPrefab;
    private void Awake()
    {
        //проверка экземпляра | синголтон | мы хотим иметь только один экземпляр глобал. ссылок 
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }




}
