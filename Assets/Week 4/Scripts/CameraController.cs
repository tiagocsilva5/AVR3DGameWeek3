using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float horz, vert;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(player);
    }

    // Update is called once per frame
    void LateUpdate()
    {


        transform.position = player.position + vert * Vector3.up - horz * player.forward;
        //has issues when camera rotates vertically, fix later
        transform.LookAt(player);
    }
}
