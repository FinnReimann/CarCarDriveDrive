using System;
using UnityEngine;

public class Tommy : MonoBehaviour
{
    private Configuration _configuration;

    private void Awake()
    {
        _configuration = GetComponent<Configuration>();
    }
}
