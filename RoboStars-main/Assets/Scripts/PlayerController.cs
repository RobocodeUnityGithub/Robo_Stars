using UnityEngine;
using UnityEngine.InputSystem; 
using Photon.Pun; // new 

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private CameraFollow myCamScript; // new
    [SerializeField] private float rotateSpeed;
    private PhotonView _pv; //new
    private PlayerInput _inputActions;
    private CharacterController _controller;
    private Aim _myAim;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector3 _currentMovement;
    private Quaternion _rotateDir;
    private bool _isRun;
    private bool _isWalk;
    private bool _isCrouch;
    
    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
    private static readonly int Throw = Animator.StringToHash("Throw");
    
    private void Awake()
    {
        _pv = GetComponentInParent<PhotonView>(); //new

        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _myAim = GetComponent<Aim>();
        _inputActions = new PlayerInput();
        _inputActions.CharacterControls.Movement.started += OnMoveActions;
        _inputActions.CharacterControls.Movement.performed += OnMoveActions;
        _inputActions.CharacterControls.Movement.canceled += OnMoveActions;

        _inputActions.CharacterControls.Run.started += OnRun;
        _inputActions.CharacterControls.Run.canceled += OnRun;

        _inputActions.CharacterControls.Crouch.started += OnCrouch;
        _inputActions.CharacterControls.Crouch.canceled += OnCrouch;

        _inputActions.CharacterControls.Movement.started += OnCameraMovement;   //new
        _inputActions.CharacterControls.Movement.performed += OnCameraMovement; //new
        _inputActions.CharacterControls.Movement.canceled += OnCameraMovement;  //new

        if (!_pv.IsMine) //new
        {
            Destroy(myCamScript.gameObject); //new
        }
    }

    void Update()
    {
        if (!_pv.IsMine) return; // new
        PlayerRotate();
        AnimateControl();
    }
    private void FixedUpdate()
    {
        if (!_pv.IsMine) return; // new 
        _controller.Move(_currentMovement * Time.fixedDeltaTime); 
    }

    private void OnEnable()
    {
        _inputActions.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _inputActions.CharacterControls.Disable();
    }

    private void OnCameraMovement(InputAction.CallbackContext context) //new
    {
        myCamScript.SetOffset(_currentMovement);     //new
    }

    private void OnMoveActions(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _currentMovement.x = _moveInput.x;
        _currentMovement.z = _moveInput.y;
        _isWalk = _moveInput.x != 0 || _moveInput.y != 0;
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        _isRun = context.ReadValueAsButton();
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        _isCrouch = context.ReadValueAsButton();
    }
    private void AnimateControl()
    {
        _animator.SetBool(IsWalk, _isWalk);
        _animator.SetBool(IsRun, _isRun);
        _animator.SetBool(IsCrouch, _isCrouch);
    }

    private void PlayerRotate()
    {
        if (_isWalk)
        {
            _rotateDir = Quaternion.Lerp(transform.rotation,
                                         Quaternion.LookRotation(_currentMovement), 
                                         Time.deltaTime * rotateSpeed);
            transform.rotation = _rotateDir;
            
        }
        else if (_myAim.GetThrowState())
        {
            transform.LookAt(_myAim.GetTargetPosition());
        }
    }
    public void Respawn()
    {
        _controller.enabled = false;
        transform.position = Vector3.up;
        _controller.enabled = true;
    }
}
