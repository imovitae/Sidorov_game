using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Weapon : MonoBehaviour
{
    public int weaponDamage; // Добавлен тип для weaponDamage
    // Ссылка на камеру игрока 
    public Camera playerCamera;

    // Стрельба 
    public bool isShooting, readyToShoot; // Стреляем или нет?
    // Логическое значение для выстрела (один раз)
    bool allowReset = true;
    public float shootingDelay = 2f; // Задержка

    public int bulletsPerBurst = 3; // Режим стрельбы 
    public int burstBulletsLeft;

    // Рассеивание или же спрей
    public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30; // Скорость пули 
    public float bulletPrefabLifeTime = 3f; // Время жизни пули
                                            //
    public GameObject MuzzleEffect; // Эффект для дула
    private Animator animator;

    // Режим стрельбы 
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    // Текущий режим стрельбы 
    public ShootingMode currentShootingMode;

    // Готовность к стрельбе 
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        // Проверка наличия компонента Animator
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the Weapon game object.");
        }
    }

    // В основном проверки стрельбы ну или же тип стрельбы, который проверяется в этом методе 
    void Update()
    {
        // Если нажал левую кнопку мыши, значит хочешь пострелять 
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }

        // Учтение различных методов стрельбы 
        if (currentShootingMode == ShootingMode.Auto)
        {
            // Удерживание кнопки 
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // Один раз нажал
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Проверка, стреляем ли мы или готовы ли мы к стрельбе 
        if (readyToShoot && isShooting)
        {
            // Текущее количество патронов при каждом разе нашей пальбы 
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    // Создание пули представляющей небольшой объект | создание пули при каждом выстреле 
    private void FireWeapon()
    {
        MuzzleEffect.GetComponent<ParticleSystem>().Play(); // Активация системы частиц при стрельбе

        // Проверка наличия компонента Animator перед его использованием
        if (animator != null)
        {
            animator.SetTrigger("RECOIL"); // Доступ к аниматору
        }

        readyToShoot = false; // Значение false останавливает возможность стрельбы 
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized; // Вектор направления стрельбы и метод который рассчитывает направление и разброс 

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity); // Создание пули в месте появления и вращение 

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        // Экземпляр пули | поворачивание оси вперед для направления выстрела 
        bullet.transform.forward = shootingDirection;

        // Приложение к пули усилия 
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Исчезновение пули в течение какого-то времени 
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime)); // Используем задержку с помощью сопрограммы

        // Возможность повторного выстрела 
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Режим пальбы Burst
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    // Возможность сброса кадра 1 раз 
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Массив для того чтобы узнать куда попадет пуля 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            // Пальба в небо, воздух 
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
