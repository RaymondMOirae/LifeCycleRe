using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPoint : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int cdTime = 100;
    int timer = 0;
    private void Update()
    {
        timer++;
        if (timer > cdTime)
        {
            timer = 0;
            Shoot();
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position + -1 * gameObject.transform.right, Quaternion.identity, this.gameObject.transform);
        bullet.GetComponent<Bullet>().parentPosition = transform.position;
        Debug.Log("222");
    }
    public void DamageAllBullet()
    {
        while (transform.childCount != 0)
        {
            Destroy(transform.GetChild(0));
        }
    }
}
