using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class dustScript : MonoBehaviour
{

    public GameObject dustParticleRun;
    public GameObject dustParticleStand;

    Input _input;
    InputAction _move;
    public Vector2 _moveDirection;

    public float emmission, lifetime;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        _input = new Input();
    }
    public void Move(Vector2 context)
    {
        _moveDirection = context;
    }

    private void OnEnable()
    {
        _input.Enable();

        _move = _input.Movement.Move;
    }
    // Update is called once per frame
    void Update()
    {
        //If player is MOVING
        Move(_move.ReadValue<Vector2>());
        if (_moveDirection != Vector2.zero)
        {
            //run particle aan stand sstill particle uit
            dustParticleRun.SetActive(true);
            dustParticleStand.SetActive(false);
        }
        else
        {
            dustParticleRun.SetActive(false);
            dustParticleStand.SetActive(true);
        }
    }
}
