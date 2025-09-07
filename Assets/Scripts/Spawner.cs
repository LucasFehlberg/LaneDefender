using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private GameObject[] enemies;
    private IEnumerator Spawn()
    {
        while (controller.GameRunning)
        {
            transform.position = new(10, Random.Range(-4, 1));
            Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1f, 3.5f));
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }
}
