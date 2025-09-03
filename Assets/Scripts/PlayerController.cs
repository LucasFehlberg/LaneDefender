using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float playerAccel = 1f;
    [SerializeField] private float weaponCooldown = 10;

    [SerializeField] private PlayerInput pInput;
    [SerializeField] private Rigidbody rb;

    float currentVelo = 0f;

    private InputAction move;
    private InputAction shoot;

    private void Awake()
    {
        pInput.currentActionMap.Enable();

        move = pInput.currentActionMap.FindAction("Move");
        shoot = pInput.currentActionMap.FindAction("Shoot");

        shoot.started += Shoot_started;
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        currentVelo = Mathf.MoveTowards(currentVelo, move.ReadValue<float>() * playerSpeed, playerAccel);

        rb.linearVelocity = new(0, currentVelo);

        transform.position = new(-8, Mathf.Clamp(transform.position.y, -4, 0));

        if (weaponCooldown > 0)
        {
            weaponCooldown--;
        }
    }
}
