using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Movement : NetworkBehaviour
{
    [SerializeField]
    private Transform cameraPrefab ,cameraSpawnLoc;

    [SerializeField]
    private GameObject playerUI;

    [SerializeField]
    private float 
        playerSpeed,
        rotateSpeed;

    private Transform cameraObject;

    private float
        horizontalInput,
        verticalInput;

    private PlayerInput plInput;

    void Start()
    {
        plInput = GetComponent<PlayerInput>();

        if (IsOwner)
        {
            if (GameObject.FindGameObjectWithTag("UI_Player") == null)
            {
                Instantiate(playerUI);

            }

            if (cameraObject == null)
            {
                cameraObject = Instantiate(cameraPrefab, cameraSpawnLoc);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Move()
    {
        if (!IsOwner) return;



        if (plInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
        {
            horizontalInput = plInput.actions["Move"].ReadValue<Vector2>().x;
            verticalInput = plInput.actions["Move"].ReadValue<Vector2>().y;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x + (horizontalInput * playerSpeed * Time.deltaTime), pos.y, pos.z + (verticalInput * playerSpeed * Time.deltaTime));
    }
}
