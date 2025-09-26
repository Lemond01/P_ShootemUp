using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;   // Prefab de la bala
    public Transform[] firePoints;    // Array de fire points
    public float bulletSpeed = 20f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = firePoint.forward * bulletSpeed; // Dispara hacia adelante
        }
    }
}


