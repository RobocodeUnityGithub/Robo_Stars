using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private CameraFlow myCamScript;
    private PlayerInput inputActions;
    private CharacterController controller;
    private Animator animator;
    private Vector2 movementInput;
    private Vector3 currentMovement;
    private Quaternion rotateDir;
    private bool isRun;
    private bool isWalk;
    private PhotonView pv;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputActions = new PlayerInput();
        // Якщо ми починаємо натискати на клавіші (started) - запустимо метод OnMovemntActions
        inputActions.CharacterControls.Movement.started += OnMovemntActions;
        // Якщо значення змінюється (актуально для gamepada)
        inputActions.CharacterControls.Movement.performed += OnMovemntActions;
        // Якщо ми перестали натискати на клавіші
        inputActions.CharacterControls.Movement.canceled += OnMovemntActions;
        inputActions.CharacterControls.Run.started += OnRun;
        inputActions.CharacterControls.Run.canceled += OnRun;
        pv = GetComponentInParent<PhotonView>();

        inputActions.CharacterControls.Movement.started += OnCameraMovement;
        inputActions.CharacterControls.Movement.performed += OnCameraMovement;
        inputActions.CharacterControls.Movement.canceled += OnCameraMovement;



        if (!pv.IsMine)
        {
            Destroy(myCamScript.gameObject);
        }

    }

    private void Update()
    {
        if (!pv.IsMine) return;
        AnimateControl();
        PlayerRotate();
    }

    public void Respawn()
    {
        controller.enabled = false;
        transform.position = Vector3.up;
        controller.enabled = true;
    }


    private void FixedUpdate()
    {
        if (!pv.IsMine) return;
        controller.Move(currentMovement * Time.fixedDeltaTime);
    }

    private void OnCameraMovement(InputAction.CallbackContext context)
    {
        myCamScript.SetOffset(currentMovement);
    }


    private void OnMovemntActions(InputAction.CallbackContext context)
    {
        // при спрацюванні метода - запишемо дані про натиснуті клавіші в movementInput
        movementInput = context.ReadValue<Vector2>();
        // оскільки натискання кнопок зчитується у Vector 2, а у нас 3D гра - перетворимо вісь Y 
        //(стрілки вгору та вниз) на вісь Z(рух вперед / назад)
        currentMovement.x = movementInput.x;
        currentMovement.z = movementInput.y;
        // якщо натиснута хоча-б одна клавіша - запишемо в isWalk true.
        isWalk = movementInput.x != 0 || movementInput.y != 0;
    }

    private void OnEnable()
    {
        inputActions.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        inputActions.CharacterControls.Disable();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }


    private void PlayerRotate()
    {
        if (isWalk)
        {
            rotateDir = Quaternion.Lerp(transform.rotation,
      Quaternion.LookRotation(currentMovement), Time.deltaTime * rotateSpeed);
            transform.rotation = rotateDir;
        }
    }
    private void AnimateControl()
    {
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);
    }


}
