using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Disables the hitbox so that way we can still use this for the tank's visual FX
    /// </summary>
    public void DisableHitbox()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().LoseHealth(1);
        }
    }
}
