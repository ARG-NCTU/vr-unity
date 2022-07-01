using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using Photon.Pun;

public class TopicPublisher : MonoBehaviour
{
    RosSocket rosSocket;
    string joystick_x;
    string joystick_y;
    string vr_joystick_xy;

    //VR Device
    public string FrameId = "Unity";
    public string WebSocketIP = "ws://192.168.0.104:9090"; //IP address
    public ControllersManager controllers;

    private string RosBridgeServerUrl; //IP address
    private PhotonView huskyPV;
    private Vector2 joyVal;
    private float x, y;
    

    // Start is called before the first frame update
    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        joystick_y = rosSocket.Advertise<std_msgs.Float32>("vr/joystick_y");
        joystick_x = rosSocket.Advertise<std_msgs.Float32>("vr/joystick_x");
        vr_joystick_xy = rosSocket.Advertise<std_msgs.Float32MultiArray>("vr/joystick_xy");
    }

    // Update is called once per frame
    void Update()
    {
        //------------------Pub_joyValue.x,y------------------------------//

        joyVal = controllers.getLeftjoyVal();
        y = joyVal.y;
        x = joyVal.x;

        //Debug.Log("y val: " + y);
        //Debug.Log("x val: " + x);
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
        message_xy.data[0] = x;
        message_xy.data[1] = y;

        rosSocket.Publish(vr_joystick_xy, message_xy);
        Debug.Log("y val: " + message_xy.data[0]);
        Debug.Log("x val: " + message_xy.data[1]);
    }
}
