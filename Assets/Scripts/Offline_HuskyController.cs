using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Offline_HuskyController : MonoBehaviour
{
    RosSocket rosSocket;
    string joystick_x;
    string joystick_y;
    string vr_joystick_xy;


    //VR Device

    public string FrameId = "Unity";
    public string WebSocketIP = "ws://10.42.0.2:9090"; //IP address

    private Transform vr_controller;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice rightController, leftController;
    private string RosBridgeServerUrl; //IP address

    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        //Debug.Log("Established connection with ros");

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
        joystick_y = rosSocket.Advertise<std_msgs.Float32>("vr/joystick_y");
        joystick_x = rosSocket.Advertise<std_msgs.Float32>("vr/joystick_x");
        vr_joystick_xy = rosSocket.Advertise<std_msgs.Float32MultiArray>("vr/joystick_xy");
    }

    void Update()
    {

        //------------------Pub_Joystick------------------------------//
        leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joyValue);
        //Debug.Log("Joy Value x " + joyValue.x);
        //Debug.Log("Joy Value y " + joyValue.y);
        float y = joyValue.y;

        Vector3 velocity = new Vector3(0, 0, y);

        velocity = transform.TransformDirection(velocity);

        velocity *= 0.5f;

        transform.localPosition += velocity * Time.fixedDeltaTime;

        float x = joyValue.x;

        transform.Rotate(0, x * 0.3f, 0);
        //------------------Pub_joyValue.x,y------------------------------//

        //string temp_trigger = triggerValue.ToString("0.000");
        std_msgs.Float32 message_y = new std_msgs.Float32
        {
            data = y
        };

        rosSocket.Publish(joystick_y, message_y);



        //string temp_trigger = triggerValue.ToString("0.000");
        std_msgs.Float32 message_x = new std_msgs.Float32
        {
            data = x
        };

        rosSocket.Publish(joystick_x, message_x);

        std_msgs.Float32MultiArray message_xy = new std_msgs.Float32MultiArray();
        message_xy.data = new float[2];
        message_xy.data[0] = 0.5f * x;
        message_xy.data[1] = 0.3f * y;

        rosSocket.Publish(vr_joystick_xy, message_xy);
        Debug.Log("y val: " + message_xy.data[0]);
        Debug.Log("x val: " + message_xy.data[1]);

    }
}
