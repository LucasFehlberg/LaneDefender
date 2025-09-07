using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float missileSpeed = 8.0f;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float defaultExplosionScale = 1f;
    [SerializeField] private float missileOnMissileScale = 2.5f;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.linearVelocity = transform.right * missileSpeed;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(rb2d.linearVelocityY, rb2d.linearVelocityX));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //We actually *don't* want to do the call here
        //We'll set up a method to re-use the explosion for dealing damage to enemies, because I think it'd be fun to strategize how you use your missiles
        //It's SUPER hard to do in practice but upon doing it, it feels good to pull off and actually hit something
        //But kinda impractical
        if (collision.collider.CompareTag("Enemy"))
        {
            GameObject explosion = Instantiate(explosionPrefab, collision.GetContact(0).point, transform.rotation);
            explosion.transform.localScale *= defaultExplosionScale;
        }

        if (collision.collider.CompareTag("Missile"))
        {
            GameObject explosion = Instantiate(explosionPrefab, collision.GetContact(0).point, transform.rotation);
            explosion.transform.localScale *= missileOnMissileScale;
        }

        Destroy(gameObject);
    }
}
