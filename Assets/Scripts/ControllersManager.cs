using RosSharp.RosBridgeClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class ControllersManager : MonoBehaviour
{
    private InputDevice rightController;
    private InputDevice leftController;
    private List<InputDevice> devices = new List<InputDevice>();

    private float gripRightValue, gripLeftValue;
    private bool primaryButtonValue, secondaryButtonValue;
    private Vector2 leftJoy;

    void Start()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }
    }


    //-------RIGHT CONTROLLER------------//
    public float getRightGrip()
    {
        //rightController.TryGetFeatureValue(CommonUsages.grip, out float gripRightValue);
        return gripRightValue;
    }

    public float getRightTrigger()
    {
        rightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        return triggerValue;
    }

    public bool getPrimaryButton()
    {
        rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryValue);
        return primaryValue;
    }

    public bool getSecondaryButton()
    {
        return secondaryButtonValue;
    }

    [PunRPC]
    public void getNetworkRightGrip(float gripRightValue)
    {
        this.gripRightValue = gripRightValue;
    }

    [PunRPC]
    public void getNetworkRightPrim(bool primaryButtonValue)
    {
        this.primaryButtonValue = primaryButtonValue;
        Debug.Log(primaryButtonValue);
    }

    [PunRPC]
    public void getNetworkRightSec(bool secondaryButtonValue)
    {
        this.secondaryButtonValue = secondaryButtonValue;
    }

    //-------------------------------------------------------//

    //-------LEFT CONTROLLER------------//
    public float getLeftGrip()
    {
        return gripLeftValue;
    }

    public Vector2 getLeftjoy() //for offline use
    {
        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joyValue);
        return joyValue;
    }

    public Vector2 getLeftjoyVal() //for online use get from RPC
    {
        return leftJoy;
    }

    [PunRPC]
    public void getNetworkLeftjoy(Vector2 leftJoy) //update left joy from RPC
    {
        this.leftJoy = leftJoy;
    }

    [PunRPC]
    public void getNetworkLeftgrip(float gripLeftValue)
    {
        this.gripLeftValue = gripLeftValue;
    }

}
