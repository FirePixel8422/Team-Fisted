using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IngameUi : MonoBehaviour
{
    Input _input;

    InputAction _tab;
    InputAction _esc;

    public GameObject _helpTab;
    public GameObject _settings;

    Movement _movement;

    private void Awake()
    {
        _input = new Input();
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _tab = _input.Ui.Tab;
        _esc = _input.Ui.Esc;

        _tab.started += Tab;
        _tab.canceled += Tab;

        _esc.started += Esc;
        _esc.canceled += Esc;
    }

    private void OnDisable()
    {
        _input.Disable();

        _tab = null;
        _esc = null;
    }

    public void Tab(InputAction.CallbackContext context)
    {
        if(context.started && _helpTab.activeInHierarchy)
        {
            _helpTab.SetActive(false);
        }

        else if (context.started && !_helpTab.activeInHierarchy)
        {
            _helpTab.SetActive(true);
        }
    }

    public void Esc(InputAction.CallbackContext context)
    {
        if (context.started && _settings.activeInHierarchy)
        {
            _settings.SetActive(false);
            _movement.enabled = true;
        }

        else if (context.started && !_settings.activeInHierarchy)
        {
            _settings.SetActive(true);
            _movement.enabled = false;
        }
    }
}
