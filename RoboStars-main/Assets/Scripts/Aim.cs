using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> allTarget;
    [SerializeField] private GameObject targetCylinder;
    [SerializeField] private AnimationClip throwAnimation;
    [SerializeField] private float range;
    private AnimatorClipInfo[] _animatorClipInfo;
    private PlayerInput _inputs;
    private PhotonView _pv;
    private Animator _animator;
    private CharacterController _controller;
    private GameObject _targetGameObject;
    private bool _canSearch = true; //new
    private bool _isThrow;
    private bool _canShoot;
    private int _targetCount;
    private float _throwTime;
    private static readonly int Throw = Animator.StringToHash("Throw");

    private void Awake()
    {
        _inputs = new PlayerInput();
        _controller = GetComponent<CharacterController>();
        _pv = GetComponentInParent<PhotonView>();
        _animator = GetComponent<Animator>();
        _throwTime = throwAnimation.length;
        
    }


    private void OnFire(InputAction.CallbackContext context)
    {

        if (_targetGameObject != null && _canShoot)
        {
            _animator.SetTrigger(Throw);
            _isThrow = true;
            StartCoroutine(FireRate());
        }
    }

    IEnumerator FireRate()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_throwTime);
        Vector3 dir = (_targetGameObject.transform.position - transform.position).normalized;

        GameObject temp = PhotonNetwork.Instantiate(Path.Combine("FireBall"),
            spawnPosition.position, Quaternion.identity);
        temp.GetComponent<Bullet>().StartMove(dir);
        Physics.IgnoreCollision(temp.GetComponent<Collider>(), transform.GetComponent<Collider>());
        _canShoot = true;
    }
    public bool GetThrowState()
    {
        return _isThrow;
    }
    
    public Vector3 GetTargetPosition()
    {
        if (_targetGameObject)
        {
            return _targetGameObject.transform.position;
        } 
        return Vector3.forward;
    }
    void Start()
    {
        if (!_pv.IsMine)
        {
            return;
        }
        targetCylinder.SetActive(false);
        _inputs.CharacterControls.ChangeTarger.started += ChangeTarget;
        _inputs.CharacterControls.Fire.started += OnFire;

        _canShoot = true;

    }
    
    private void OnEnable()
    {
        _inputs.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        _inputs.CharacterControls.Disable();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    public void SetTargetStatus(bool isTarget)
    {
        targetCylinder.SetActive(isTarget);
    }
    
    private void SelectTarget()
    {
        if (_controller.velocity == Vector3.zero)
        {
            if (_canSearch)
            {
                InvokeRepeating(nameof(Calculate), 0f, 0.5f); 
            }
        }
        else
        {
            if (_targetGameObject != null && !_pv.IsMine)
            {
                targetCylinder.GetComponent<Aim>().SetTargetStatus(false);
                _targetGameObject = null; // new
            }
    
            _canSearch = true; //
            CancelInvoke();
        }
    }
    
    private void Calculate()
    {
        _canSearch = false;
        allTarget.Clear();
    
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.position, range);
        foreach (RaycastHit hit in hits)
        {
            GameObject tempObj = hit.collider.gameObject;
            if (tempObj.GetComponent<CharacterController>() && !tempObj.GetComponentInParent<PhotonView>().IsMine)
            {
                allTarget.Add(tempObj);
            }
            else
            {
                continue;
            }
        }
        SelectNewTarget();
    }
    
    private void ChangeTarget(InputAction.CallbackContext context)
    {
        _targetCount++;
        SelectNewTarget();
    }
    
    private void SelectNewTarget()
    {
        foreach (GameObject obj in allTarget)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
    
        if (_targetCount >= allTarget.Count)
        {
            _targetCount = 0; 
        }

        if (allTarget.Count != 0) //add
        {
            _targetGameObject = allTarget[_targetCount];
            allTarget[_targetCount].GetComponent<Aim>().SetTargetStatus(true);
        }
        
    }
    
    void FixedUpdate()
    {
        
        if (!_pv.IsMine)
        {
            return;
        }

        _isThrow = false;
        SelectTarget();
    }

    private void SelectNewTarget(InputAction.CallbackContext context)
    {
        _targetCount++;
        foreach (GameObject obj in allTarget)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
        if (_targetCount >= allTarget.Count)
        {
            _targetCount = 0;
        }
       _targetGameObject = allTarget[_targetCount];
        allTarget[_targetCount].GetComponent<Aim>().SetTargetStatus(true);
    }
}
