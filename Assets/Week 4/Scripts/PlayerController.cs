using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed, runSpeed, gravity, jumpSpeed, turnSpeed;


    CharacterController controller;
    Vector2 input;
    float vertVelo;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        vertVelo = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 velo = (input.x * transform.right + input.y * transform.forward);
        velo = velo.normalized;
        velo *= Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if(controller.isGrounded)
        {
            vertVelo = Input.GetKeyDown(KeyCode.Space) ? jumpSpeed : -5f;
        }
        else
        {
            vertVelo -= gravity * Time.deltaTime;
        }
        velo += vertVelo * Vector3.up;
        controller.Move(velo * Time.deltaTime);

        //Player rotation
        transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
    }
}
