using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(RotatePlayerAndMapSlow(collision.gameObject));
            Time.timeScale = 0.5f;
        }
    }
    IEnumerator RotatePlayerAndMapSlow(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        for (int i = 0; i < 9; i++)
        {
            yield return new WaitForSeconds(0.03f);
            GameManager.gameManager.RotateMap(10);
            player.transform.RotateAround(new Vector3(9, 9, 0), new Vector3(0, 0, 1), 10);
            player.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;//hack:可以去？
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //hack：测试
    }
}
