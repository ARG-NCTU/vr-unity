using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using sensor_msgs = RosSharp.RosBridgeClient.MessageTypes.Sensor;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class Husky_Switch_Control : MonoBehaviour
{
    RosSocket rosSocket;
    string publication_test; //Pub_test
    string primary_button;
    string scondary_button;
    string grip;
    string trigger;
    string joystick_xy;

    //VR Device
    public string FrameId = "Unity";
    public string WebSocketIP = "ws://10.42.0.4:9090"; //IP address

    public ControllersManager controllerInput;
    private Vector2 joyValue;
    private string RosBridgeServerUrl; //IP address

    string Rightprimary_button, Leftprimary_button;
    string Rightscondary_button, Leftscondary_button;
    string Rightgrip_button, Leftgrip_button;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;
    private bool LeftprimaryButtonValue, LeftsecondaryButtonValue;
    private float gripRightValue, gripLeftValue;

    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        joystick_xy = rosSocket.Advertise<std_msgs.Float32MultiArray>("vr/joystick_xy");
        Rightprimary_button = rosSocket.Advertise<std_msgs.Bool>("/vr/right/primarybutton");
        Rightscondary_button = rosSocket.Advertise<std_msgs.Bool>("/vr/right/secondarybutton");
        Leftprimary_button = rosSocket.Advertise<std_msgs.Bool>("/vr/left/primarybutton");
        Leftscondary_button = rosSocket.Advertise<std_msgs.Bool>("/vr/left/secondarybutton");
        Rightgrip_button = rosSocket.Advertise<std_msgs.Float32>("/vr/right/gripbutton");
        Leftgrip_button = rosSocket.Advertise<std_msgs.Float32>("/vr/left/gripbutton");

    }

    void Update()
    {

        //------------------Pub_left_Joystick------------------------------//
        joyValue = controllerInput.getLeftjoy(); //get joyvalue from mananger
        //Debug.Log("Joy Value x " + joyValue.x);
        //Debug.Log("Joy Value y " + joyValue.y);
        float y = joyValue.y;
        float x = joyValue.x;

        //transform.Rotate(0, x * 0.3f, 0);
        //------------------Pub_joyValue.x,y------------------------------//
        std_msgs.Float32MultiArray message_xy = new std_msgs.Float32MultiArray();
        message_xy.data = new float[2];

        message_xy.data[0] = x;
        message_xy.data[1] = y;

        rosSocket.Publish(joystick_xy, message_xy);
        Debug.Log("Joy Value x " + joyValue.x);
        Debug.Log("Joy Value y " + joyValue.y);

        //------------------Pub_Primary Buttom------------------------------//

        RightprimaryButtonValue = controllerInput.getRightPrimaryButton();
        std_msgs.Bool message_p = new std_msgs.Bool
        {
            data = RightprimaryButtonValue
        };

        rosSocket.Publish(Rightprimary_button, message_p);

        LeftprimaryButtonValue = controllerInput.getLeftPrimaryButton();
        std_msgs.Bool message_p_l = new std_msgs.Bool
        {
            data = LeftprimaryButtonValue
        };

        rosSocket.Publish(Leftprimary_button, message_p_l);
        //Debug.Log("LeftprimaryButtonValue" + LeftprimaryButtonValue);

        //------------------Pub_Secondary Buttom------------------------------//

        RightsecondaryButtonValue = controllerInput.getRightSecondaryButton();
        std_msgs.Bool message_s = new std_msgs.Bool
        {
            data = RightsecondaryButtonValue
        };

        rosSocket.Publish(Rightscondary_button, message_s);

        LeftsecondaryButtonValue = controllerInput.getLeftSecondaryButton();
        std_msgs.Bool message_s_l = new std_msgs.Bool
        {
            data = LeftsecondaryButtonValue
        };

        rosSocket.Publish(Leftscondary_button, message_s_l);
        //Debug.Log("LeftsecondaryButtonValue" + LeftsecondaryButtonValue);

        //------------------Pub_Grip Buttom------------------------------//
        gripRightValue = controllerInput.getRightGrip();
        std_msgs.Float32 message_r_g = new std_msgs.Float32
        {
            data = gripRightValue
        };
        rosSocket.Publish(Rightgrip_button, message_r_g);

        gripLeftValue = controllerInput.getLeftGrip();
        std_msgs.Float32 message_l_g = new std_msgs.Float32
        {
            data = gripLeftValue
        };
        rosSocket.Publish(Leftgrip_button, message_l_g);
        //Debug.Log("gripLeftValue" + gripLeftValue);

    }
}
