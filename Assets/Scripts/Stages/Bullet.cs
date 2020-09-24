using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 parentPosition;
    private void Start()
    {
        StartCoroutine(Disappear());
    }
    private void Update()
    {
        Vector2 temp = ((Vector2)transform.position - (Vector2)transform.parent.transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = 5 * temp;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<PlayerController>().dead)
        {
            collision.gameObject.GetComponent<PlayerController>().deadSign();
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<AudioSource>().Play();
            StartCoroutine(WaitForDie(collision.gameObject));
            StartCoroutine(Effect(collision.gameObject));
        }
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(5);
        DamageBullet();
    }
    private void DamageBullet()
    {
        Destroy(this.gameObject);
    }
    IEnumerator WaitForDie(GameObject player)
    {
        yield return new WaitForSeconds(2.2f);
        Debug.Log("hurt___");
        player.GetComponent<PlayerController>().TakeDamage();
    }
    IEnumerator Effect(GameObject player)
    {
        Debug.Log("effect");
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
