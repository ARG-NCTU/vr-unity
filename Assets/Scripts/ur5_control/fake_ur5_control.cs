using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using RosSharp.RosBridgeClient;

public class fake_ur5_control : MonoBehaviour
{
    public GameObject target_ee;
    public GameObject target_wrist_pitch;
    public GameObject target_wrist_yaw;
    private float xVR, zVR;

    private Transform vr_controller;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice rightController, leftController;

    void Start()
    {
        //XRRig rig = FindObjectOfType<XRRig>();
        //vr_controller = rig.transform.Find("Camera Offset/RightHand Controller");
        var go = GameObject.Find("XRRig/Camera Offset/RightHand Controller");

        if(go == null)
        {
            Debug.LogError("Cannot find XRRig/Camera Offset/RightHand Controller");
            return;
        }

        vr_controller = go.transform;

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }
    }

    void Update()
    {
        //Debug.Log("x" + vr_controller.position.x);
        //Debug.Log("y" + vr_controller.position.y);
        //Debug.Log("z" + vr_controller.position.z);
        zVR = vr_controller.eulerAngles.z;
        xVR = vr_controller.eulerAngles.x;

        target_ee.transform.position = vr_controller.position;
        target_ee.transform.rotation = vr_controller.rotation;

        target_wrist_pitch.transform.eulerAngles = new Vector3(xVR, transform.eulerAngles.y, transform.eulerAngles.z);
        target_wrist_yaw.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zVR);
    }
}
