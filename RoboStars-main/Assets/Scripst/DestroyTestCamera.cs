using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTestCamera : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
