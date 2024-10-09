using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterect : MonoBehaviour
{
    Input _input;

    InputAction _interect;

    private void Awake()
    {
        _input = new Input();
    }

    private void OnEnable()
    {
        _input.Enable();

        _interect = _input.Interect.Interect;

        _interect.started += Interect;
    }

    private void OnDisable()
    {
        _input.Disable();

        _interect = null;
    }

    void Interect(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        if(Physics.Raycast(GetComponent<Movement>()._components._camera.transform.position, GetComponent<Movement>()._components._camera.transform.forward, out hit, 5))
        {
            if (hit.transform.GetComponent<ObjectInterect>())
            {
                hit.transform.GetComponent<ObjectInterect>().Interect();
            }
        }
    }
}
