using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int health;

    [SerializeField] AudioSource hit;
    [SerializeField] AudioSource death;

    [SerializeField] private int score;

    private Vector3 mostRecentCollisionPoint;

    [SerializeField] GameObject[] spawnOnKill;

    private void Start()
    {
        StartCoroutine(Freeze());
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            SpawnOnKill();
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().linearVelocity = -mostRecentCollisionPoint * 50f;
            death.Play();

            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().UpdateScore(score);
            return;
        }

        hit.Play();
        GetComponent<Animator>().SetTrigger("Hit");
        //GetComponent<Animator>().ResetTrigger("Hit");
        StopAllCoroutines();
        StartCoroutine(Freeze());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mostRecentCollisionPoint = collision.contacts[0].point - (Vector2)transform.position;
    }

    private IEnumerator Freeze()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);
        if (health <= 0)
        {
            yield break;
        }
        GetComponent<Rigidbody2D>().linearVelocity = new(-speed, 0);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void SpawnOnKill()
    {
        for (int i = 0; i < spawnOnKill.Length; i++)
        {
            Vector3 position = new(transform.position.x + ((i + 1) * 2), transform.position.y);

            Instantiate(spawnOnKill[i], position, Quaternion.identity);
        }
    }
}
