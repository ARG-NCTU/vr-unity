using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HuskyController : MonoBehaviour
{
    
    /*string publication_test; //Pub_test
    string primary_button;
    string scondary_button;
    string grip;
    string trigger;*/
    
    public float speed = 0.5f;
    public ControllersManager controllerInput;
    private Vector2 joyValue;
    private PhotonView huskyPV;

    private void Start()
    {
        huskyPV = GetComponent<PhotonView>();
    }

    //responisble for controlling husky in Unity + publishing joy values over Pun2 => publish to ROS in Topic Publisher script
    void Update()
    {
        //------------------Pub_Joystick------------------------------//

        //Debug.Log("Joy Value x " + joyValue.x);
        //Debug.Log("Joy Value y " + joyValue.y);

        joyValue = controllerInput.getLeftjoy(); //get joyvalue from mananger
        float y = joyValue.y;
        float x = joyValue.x;
        
        if(huskyPV.IsMine)
        {
            Vector3 velocity = new Vector3(0, 0, y);
            

            velocity = transform.TransformDirection(velocity);
            velocity *= speed;
            transform.localPosition += velocity * Time.fixedDeltaTime;
            transform.Rotate(0, x * 0.3f, 0);

            huskyPV.RPC("getNetworkLeftjoy", RpcTarget.All, joyValue);
        }
    }
}
