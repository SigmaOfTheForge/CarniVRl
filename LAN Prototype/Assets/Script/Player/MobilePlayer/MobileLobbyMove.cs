using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileLobbyMove : Movement
{
    [SerializeField]
    private float turnSpeed;

    private float horizontalLookInput;


    public override void Update()
    {
        base.Update();
        Look();
    }


    private void Look()
    {
        if (!IsOwner || !canMove.Value) return;


        if (plInput.actions["Look"].ReadValue<Vector2>() != Vector2.zero)
        {
            horizontalLookInput = plInput.actions["Look"].ReadValue<Vector2>().x * turnSpeed * Time.deltaTime;
        }
        else
        {
            horizontalLookInput = 0;
        }


        transform.Rotate(Vector3.up * horizontalLookInput);

    }
}
