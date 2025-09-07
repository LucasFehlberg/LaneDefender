using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;

    public float ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }

    private void Update()
    {
        transform.position -= Vector3.right * Time.deltaTime * scrollSpeed;

        if(transform.position.x < -1.5)
        {
            transform.position += Vector3.right * 4f;
        }
    }
}
