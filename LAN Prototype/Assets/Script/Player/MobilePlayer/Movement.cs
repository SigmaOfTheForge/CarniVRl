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
        playerSpeed;

    protected NetworkVariable<bool> canMove = new NetworkVariable<bool>(true);

    private Transform cameraObject;

    private float
        horizontalInput,
        verticalInput;
        

    protected PlayerInput plInput;

    void Start()
    {
        plInput = GetComponent<PlayerInput>();

        if (IsOwner)
        {
            //instantiating UI and Camera locally so that only this player has access to them
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

    public void ToggleMove()
    {
        canMove.Value = !canMove.Value;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CameraCheck();
        Move();
    }

    void CameraCheck() //ensures the main camera always remains the player's camera to avoid issues when other players join
    {
        if(!IsOwner) return;

        Camera cam = Camera.main;
        if (cameraObject != cam)
        {
            cam.tag = "Untagged";
            Destroy(cameraObject.gameObject);
            cameraObject = Instantiate(cameraPrefab, cameraSpawnLoc);
        }
    }


    void Move() //move the player based on screen joystick input
    {
        if (!IsOwner || !canMove.Value) return;



        if (plInput.actions["Move"].ReadValue<Vector2>() != Vector2.zero)
        {
            horizontalInput = plInput.actions["Move"].ReadValue<Vector2>().x;
            verticalInput = plInput.actions["Move"].ReadValue<Vector2>().y;
        }
        else
        {
            horizontalInput = 0;
            verticalInput = 0;
        }

        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        
        

        Vector3 pos = transform.position;
        transform.position = pos + (movement * playerSpeed  * Time.deltaTime);
    }
}
