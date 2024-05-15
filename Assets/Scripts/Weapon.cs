using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;//скорость пули 
    public float bulletPrefabLifeTime = 3f;//врем€ жизни пули     

    // Update is called once per frame
    void Update()
    {
        //если нажал левую кнопку мыши , значит хочешь пострел€ть 
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            FireWeapon();
        }
    }
    //создание пули предстовал€ющей небол. объект | создание пули при каждом выстреле 
    private void FireWeapon()
    {

        GameObject bullet=Instantiate(bulletPrefab, bulletSpawn.position,Quaternion.identity);//создание пули в месте по€лвени€ и вращение 
        //син€€ ось за вылет пули 
        //ѕриклоадываем к пули усили€ . берем компонент твердого тела который находитс€ на теле и прикладываем к нему усили€ и прикладываем ему направление | пул€ вылетит веперед и умножаем на скорость пули
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        //исчезновение пули в течение какого-то времени 
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime)); //»спользуем задержку с помощью сопрограммы |DestroyBulletAfterTime|
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//—опрограмма (а не обычный метод), поэтому используем IEnumerator
    {
       yield return new WaitForSeconds(delay);//врем€ жизни пули 
        Destroy(bullet);//уничтожение пули через 3  сек
    }
}
