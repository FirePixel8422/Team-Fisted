using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInterect : MonoBehaviour
{
    Input _input;

    InputAction _interect;

    public LayerMask _interectLayer;

    public GameObject _interectUi;

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

    private void FixedUpdate()
    {
        LookAt();
    }

    ObjectInterect _lasthit;

    public void LookAt()
    {
        RaycastHit hit;

        if (Physics.Raycast(GetComponent<Movement>().camera.transform.position, GetComponent<Movement>().camera.transform.forward, out hit, 10, _interectLayer))
        {
            _lasthit = hit.transform.gameObject.GetComponent<ObjectInterect>();
            _lasthit._interactUi.SetActive(true);
        }

        else if(_lasthit)
        {
            _lasthit._interactUi.SetActive(false);
            _lasthit = null;
        }
    }

    void Interect(InputAction.CallbackContext context)
    {
        RaycastHit hit;

        if (Physics.Raycast(GetComponent<Movement>().camera.transform.position, GetComponent<Movement>().camera.transform.forward, out hit, 10, _interectLayer))
        { 
            Debug.Log("Interect");
            Debug.Log(hit.transform.name);
        
            if (hit.transform.GetComponent<ObjectInterect>())
            {
                hit.transform.GetComponent<ObjectInterect>().Interect();
            }
        }
    }
}
