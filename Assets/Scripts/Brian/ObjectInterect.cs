using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInterect : MonoBehaviour
{
    public GameObject _interactUi;

    private void Start()
    {
        if (!_interactUi)
        {
            _interactUi = GetComponentInChildren<Canvas>().gameObject;
        }

        _interactUi.SetActive(false);

        StartV();
    }

    virtual public void StartV()
    {

    }

    virtual public void Interect()
    {

    }
}
