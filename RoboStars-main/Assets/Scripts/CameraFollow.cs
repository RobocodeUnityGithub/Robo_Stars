using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float rearDistance;
    [SerializeField] private float forwardZOffset = 10;
    [SerializeField] private float backwardZOffset = 3;
    [SerializeField] private float horizontalOffset = 8;
    private Vector3 cameraOffset;
    private Vector3 currentVector;
   
    void Start()
    {
        transform.position = new Vector3(mainCharacter.transform.position.x,
                                         mainCharacter.transform.position.y + height,
                                         mainCharacter.transform.position.z - rearDistance);

        transform.rotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
        //transform.rotation *= new Quaternion(0,-1,0,1); //new
    }

    public void SetOffset(Vector3 offset)
    {
        if (offset.z < 0)
        {
            cameraOffset = offset * forwardZOffset;
        }
        else if (offset.z > 0)
        {
            cameraOffset = offset * backwardZOffset;
        }
        else
        {
            cameraOffset = offset * horizontalOffset;
        }
    }
    void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        currentVector = new Vector3(mainCharacter.transform.position.x + cameraOffset.x,
                                    mainCharacter.transform.position.y + height,
                                    mainCharacter.transform.position.z - rearDistance + cameraOffset.z);

        transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
    }
}
