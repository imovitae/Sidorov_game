using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //ссылка на камеру проигрователя 
    public Camera playerCamera;

    //стрельба 
    public bool isShooting, readyToShoot; //стрелям или не?
    //логическое значения для выстрела (один раз)
    bool allowReset = true;
    public float shootingDelay = 2f;//задержка


    public int bulletsPerBurst = 3;//режим стрельбы 
    public int burstBulletsLeft;

    //рассеивание или же спрей
    public float spreadIntensity;


    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;//скорость пули 
    public float bulletPrefabLifeTime = 3f;//время жизни пули
                                           //
//режим стрельбы 
    public enum ShootingMode
    {
        Single ,
        Burst ,
        Auto
    }

    //текущий режим стрельбы 
    public ShootingMode currentShootingMode;


    //готвность к стрельбе 
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }



  //  В основном проверки стрельбы ну или же тип стрельбы , который проверяется в этом методе 
    void Update()
    {
        //если нажал левую кнопку мыши , значит хочешь пострелять 
      /*  if (Input.GetKeyDown(KeyCode.Mouse0)) {
            FireWeapon();
        }*/


      //учитвание различных методов стрельбы 
        if (currentShootingMode == ShootingMode.Auto)
        {
            //удерживание кнопки 
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode==ShootingMode.Single || currentShootingMode==ShootingMode.Burst)
        {
            //один раз нажал
            isShooting=Input.GetKeyDown(KeyCode.Mouse1);
        }

        //проверка , стреляем ли мы или готовы ли мы к стрельбе 
        if (readyToShoot && isShooting )
        {
            //текущее кол-во патронов при каждом разе нашей пальбы 
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

  
    //создание пули предстоваляющей небол. объект | создание пули при каждом выстреле 
    private void FireWeapon()
    {

        readyToShoot=false; //значения false так сказать останавливает такую возможность вереницы пуль во время пальбы 
        Vector3 shootingDirection= CalculateDirectionAndSpread().normalized;    //вектор направления стрельбы и метод который рассчитывает направление и разброс 


        GameObject bullet=Instantiate(bulletPrefab, bulletSpawn.position,Quaternion.identity);//создание пули в месте поялвения и вращение 

        //экземпляр пули | поворачивание оси вперед для направления выстрела 
        bullet.transform.forward = shootingDirection;

        //синяя ось за вылет пули 
        //Приклоадываем к пули усилия . берем компонент твердого тела который находится на теле и прикладываем к нему усилия и прикладываем ему направление | пуля вылетит веперед и умножаем на скорость пули
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        //исчезновение пули в течение какого-то времени 
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime)); //Используем задержку с помощью сопрограммы |DestroyBulletAfterTime|


        //возможность повторного выстрела 
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);

            allowReset = false;
        }

        //Режим пальбы Busrt
        if (currentShootingMode==ShootingMode.Burst && burstBulletsLeft > 1 )//если это так , то мы находимся в "очереди"
        {
            burstBulletsLeft--;//уменьшение "очереди"
            Invoke("FireWeapon", shootingDelay);//принимаем метод огнестрельного оружия для дальнейшей пальбы 
        }
    }
    //возможность сброса кадра 1 раз 
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        //массив для того чтобы узнать куда попадет пуля 
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray,out hit ))//  если попали в цель - сохраняем в этой точке
        {
            targetPoint = hit.point;
        }
        else
        {
            //пальба в небо , воздух 
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;//вычисление точки прицеливания 
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);//интенсивность Spread
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);//интенсивность Spread
        return direction + new Vector3(x,y,0);  //вычисление направления и разброса 
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//Сопрограмма (а не обычный метод), поэтому используем IEnumerator
    {
       yield return new WaitForSeconds(delay);//время жизни пули 
        Destroy(bullet);//уничтожение пули через 3  сек
    }
}
