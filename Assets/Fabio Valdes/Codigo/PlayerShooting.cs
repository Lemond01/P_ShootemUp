using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float bulletSpeed = 20f;

    private EnemyRef currentTarget;
    private bool canShoot = false;

    void Update()
    {
        // Ya no usamos Space para disparar
        // La habilidad de disparar se activa cuando hay un patrón completo
    }

    public void SetTargetAndShoot(EnemyRef target)
    {
        currentTarget = target;
        canShoot = true;
        StartCoroutine(ShootAtTarget());
    }

    IEnumerator ShootAtTarget()
    {
        if (currentTarget._enemy == null || !canShoot) yield break;

        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Calcular dirección hacia el enemigo
            Vector3 direction = (currentTarget._enemy.transform.position - firePoint.position).normalized;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = direction * bulletSpeed;

            // Configurar la bala para que se destruya al impactar
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(currentTarget._enemy.gameObject);
            }
        }

        canShoot = false;
        yield return null;
    }
}