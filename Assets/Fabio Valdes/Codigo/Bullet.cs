using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject target;
    private bool hasTarget = false;

    public void SetTarget(GameObject enemyTarget)
    {
        target = enemyTarget;
        hasTarget = true;
    }

    void Update()
    {
        if (hasTarget && target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            GetComponent<Rigidbody>().linearVelocity = direction * GetComponent<Rigidbody>().linearVelocity.magnitude;
        }
        else if (hasTarget && target == null)
        {
            // El objetivo fue destruido, destruir esta bala tambi�n
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTarget && other.gameObject == target)
        {
            // Aplicar da�o al enemigo
            EnemyTest enemy = other.GetComponent<EnemyTest>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }

            Destroy(gameObject);
        }
        else if (!hasTarget)
        {
            // Destruir bala si no tiene objetivo espec�fico
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

