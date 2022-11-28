﻿using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public static WindowsManager Layout;
    [SerializeField] private GameObject[] windows;
    private void Awake()
    {
        Layout = this;
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }
    public void OpenLayuot(string name)
    {
        foreach (GameObject window in windows)
        {
            if (window.name == name) window.SetActive(true);
            else window.SetActive(false);
        }
    }
    void Start()
    {
        OpenLayuot("Loading");
    }
}
