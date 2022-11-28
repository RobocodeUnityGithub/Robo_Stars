using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{    void Start()
    {
        GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
    }
}
