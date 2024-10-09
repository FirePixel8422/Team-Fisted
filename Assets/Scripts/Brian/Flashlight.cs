using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    Input _input;

    InputAction _flashLight;

    public Light _light;

    private void Awake()
    {
        _input = new Input();
    }

    private void OnEnable()
    {
        _input.Enable();

        _flashLight = _input.Interect.FlashLight;

        _flashLight.started += FlashLightSwitch;
    }

    private void OnDisable()
    {
        _input.Disable();

        _flashLight = null;
    }

    public void FlashLightSwitch(InputAction.CallbackContext context)
    {
        if (_light.isActiveAndEnabled)
        {
            _light.enabled = false;
        }

        else
        {
            _light.enabled = true;
        }
    }
}
