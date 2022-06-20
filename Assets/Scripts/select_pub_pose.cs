using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geo_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class select_pub_pose : MonoBehaviour
{
    [SerializeField] private Material SetectedMaterial;
    [SerializeField] private Material DefaultMaterial;
    //private Material DefaultMaterial = null;
    private Renderer objectRenderer = null;

    RaycastHit hit;
    private Transform _selection = null;
    public Transform rightController;

    RosSocket rosSocket;
    string publication_test; //Pub_test
    string ray_target;

    //VR Device
    public string FrameId = "Unity";
    public string WebSocketIP = "ws://10.42.0.4:9090"; //IP address
    public ControllersManager controllerInput;
    public string Topic_name = "vr/ray_target"; //topic name
    private string RosBridgeServerUrl; //IP address

    string Rightprimary_button, Leftprimary_button;
    string Rightscondary_button, Leftscondary_button;
    string Rightgrip_button, Leftgrip_button;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;
    private bool LeftprimaryButtonValue, LeftsecondaryButtonValue;
    private float gripRightValue, gripLeftValue, rightTriggerValue;

    private geo_msgs.Pose target_msg = new geo_msgs.Pose();
    int flag_ray = 0;

    void Start()
    {
        //RosSocket
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros");

        //Topic name
        ray_target = rosSocket.Advertise <geo_msgs.Pose> (Topic_name);
    }
    void Update()
    {
        if (_selection != null)
        {
            if (objectRenderer == null)
            {
                objectRenderer = _selection.GetComponent<Renderer>();
            }

            if (objectRenderer != null && DefaultMaterial != null)
            {
                objectRenderer.material = DefaultMaterial;
            }
            _selection = null;
        }

        Ray ray = new Ray(rightController.position, rightController.forward);
        //-------RIGHT CONTROLLER------------//
        //RightprimaryButtonValue = controllerInput.getRightPrimaryButton();
        rightTriggerValue = controllerInput.GetComponent<ControllersManager>().getRightTrigger();
        std_msgs.Bool message_p = new std_msgs.Bool
        {   data = RightprimaryButtonValue  };
        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider.tag == "target" && rightTriggerValue > 0.7f )
            {
                var selection = hit.transform;
                objectRenderer = selection.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    if (DefaultMaterial == null)
                    {
                        DefaultMaterial = objectRenderer.material;
                    }
                    objectRenderer.material = SetectedMaterial;
                }
                float distance = hit.distance;
                //Debug.Log(distance); // Distance from the controller_transfrom.forward
                Transform transform_f_controll = hit.transform;
                Debug.Log(transform_f_controll.position.ToVector3f()); // hit_Object_global_transform
                _selection = selection;
                target_msg.position.x = transform_f_controll.position.z;
                target_msg.position.y = -transform_f_controll.position.x;
                target_msg.position.z = transform_f_controll.position.y;
                flag_ray = 1;
                rosSocket.Publish(ray_target, target_msg);
            }
        }

        //------------------Pub_Primary Buttom------------------------------//

        /*RightprimaryButtonValue = controllerInput.getRightPrimaryButton();
        std_msgs.Bool message_p = new std_msgs.Bool
        {
            data = RightprimaryButtonValue
        };
        Debug.Log("RightprimaryButtonValue" + RightprimaryButtonValue);
        if (RightprimaryButtonValue == true && flag_ray == 0)
        {
            flag_ray = 1;
        }
        else if (RightprimaryButtonValue == false && flag_ray == 1)
        {
            flag_ray = 0;
        }*/
        

        //LeftprimaryButtonValue = controllerInput.getLeftPrimaryButton();
        //std_msgs.Bool message_p_l = new std_msgs.Bool
        //{
            //data = LeftprimaryButtonValue
        //};
        
        //------------------Pub_Secondary Buttom------------------------------//

        //RightsecondaryButtonValue = controllerInput.getRightSecondaryButton();
        //std_msgs.Bool message_s = new std_msgs.Bool
        //{
            //data = RightsecondaryButtonValue
        //};


        //LeftsecondaryButtonValue = controllerInput.getLeftSecondaryButton();
        //std_msgs.Bool message_s_l = new std_msgs.Bool
        //{
            //data = LeftsecondaryButtonValue
        //};

    }
}
