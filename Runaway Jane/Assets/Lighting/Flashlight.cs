using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    private Light2D flashlight;

    void Start()
    {
        flashlight = GetComponentInChildren<Light2D>();
        if (flashlight == null)
        {
            Debug.LogError("No Light2D component found on this GameObject!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
