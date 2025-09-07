using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float playerAccel = 1f;
    [SerializeField] private float weaponCooldown = 60;

    [SerializeField] private PlayerInput pInput;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject missilePrefab;

    [SerializeField] private GameObject muzzleGameObject;
    [SerializeField] private GameObject turretRotation;

    float currentVelo = 0f;
    float currentRotation = 0f;

    private InputAction move;
    private InputAction shoot;
    private InputAction rotate;

    private InputAction restart;
    private InputAction quit;

    private bool shooting = false;

    private bool awoken = false;
    private void Awake()
    {
        pInput.currentActionMap.Enable();

        move = pInput.currentActionMap.FindAction("Move");
        shoot = pInput.currentActionMap.FindAction("Shoot");

        rotate = pInput.currentActionMap.FindAction("RotateCannon");

        restart = pInput.currentActionMap.FindAction("Restart");
        quit = pInput.currentActionMap.FindAction("Quit");

        shoot.started += Shoot_started;
        shoot.canceled += Shoot_canceled;

        quit.started += Quit_started;
        restart.started += Restart_started;

        awoken = true;
    }

    private void Restart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(0);
    }

    private void Quit_started(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    private void Shoot_canceled(InputAction.CallbackContext obj)
    {
        shooting = false;
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        shooting = true;
    }

    private void OnDestroy()
    {
        if (awoken)
        {
            shoot.started -= Shoot_started;
            shoot.canceled -= Shoot_canceled;
            restart.started -= Restart_started;
            quit.started -= Quit_started;
        }
    }

    private void FixedUpdate()
    {
        //Handle movement
        currentVelo = Mathf.MoveTowards(currentVelo, move.ReadValue<float>() * playerSpeed, playerAccel);

        rb.linearVelocity = new(0, currentVelo);

        //Clamp player position
        transform.position = new(-8, Mathf.Clamp(transform.position.y, -4, 0));

        //Handle rotation
        float rot = -rotate.ReadValue<float>();
        if(rot != 0)
        {
            //Should grant max and min angles of 90 and 0
            currentRotation = Mathf.MoveTowards(currentRotation, (rot * 45f) + 45f, 2.5f);
            turretRotation.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }


        //Handle shooting
        if (weaponCooldown > 0)
        {
            weaponCooldown--;
        } 
        else if(shooting)
        {
            weaponCooldown = 20;

            GameObject explo = Instantiate(explosionPrefab, muzzleGameObject.transform.position, Quaternion.identity);
            explo.GetComponent<Explosion>().DisableHitbox();

            GameObject missile = Instantiate(missilePrefab, muzzleGameObject.transform.position, Quaternion.identity);
            missile.transform.rotation = turretRotation.transform.rotation;

            GetComponent<AudioSource>().Play();

        }
    }
}
