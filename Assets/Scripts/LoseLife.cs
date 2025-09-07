using UnityEngine;

public class LoseLife : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LoseLife();
            Destroy(collision.gameObject);
        }
    }
}
