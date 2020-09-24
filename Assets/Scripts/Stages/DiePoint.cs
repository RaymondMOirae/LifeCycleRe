using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hurt");
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<AudioSource>().Play();
            StartCoroutine(WaitForDie(collision.gameObject));
            StartCoroutine(Effect(collision.gameObject));
        }
    }
    IEnumerator WaitForDie(GameObject player)
    {
        yield return new WaitForSeconds(2.2f);
        Debug.Log("hurt");
        player.GetComponent<PlayerController>().TakeDamage();
    }
    IEnumerator Effect(GameObject player)
    {
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}
