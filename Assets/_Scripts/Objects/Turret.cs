using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform player;
    public float maxRange = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;

    private bool isFiring = false; // Ateşleme durumunu kontrol etmek için bir bayrak
    private GameObject activeBullet;

    private void Update()
    {
        Vector3 turretPosition = transform.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(turretPosition, playerPosition);
        if (distance <= maxRange && !isFiring)
        {
            if (activeBullet == null)
            {
                Fire();
            }
            else
            {
              
                Vector3 targetDirection = (playerPosition - activeBullet.transform.position).normalized;
                activeBullet.GetComponent<Rigidbody>().velocity = targetDirection * bulletSpeed;
            }
        }
    }

    private void Fire()
    {
        isFiring = true;
        
        activeBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        activeBullet.GetComponent<Bullet>().SetTarget(player);
        Destroy(activeBullet, maxRange / bulletSpeed);
        Invoke("ResetFiring", maxRange / bulletSpeed);
    }

    private void ResetFiring()
    {
        isFiring = false; 
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}