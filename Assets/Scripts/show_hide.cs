using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class show_hide : MonoBehaviour
{
    //VR Device
    public ControllersManager controllerInput;
    private Vector2 joyValue;

    string Rightprimary_button, Leftprimary_button;
    string Rightscondary_button, Leftscondary_button;
    string Rightgrip_button, Leftgrip_button;
    private bool RightprimaryButtonValue, RightsecondaryButtonValue;
    private bool LeftprimaryButtonValue, LeftsecondaryButtonValue;
    private float gripRightValue, gripLeftValue;
    int flag_hide = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //------------------Pub_Primary Buttom------------------------------//

        RightprimaryButtonValue = controllerInput.getRightPrimaryButton();
        std_msgs.Bool message_p = new std_msgs.Bool
        {
            data = RightprimaryButtonValue
        };

        LeftprimaryButtonValue = controllerInput.getLeftPrimaryButton();
        std_msgs.Bool message_p_l = new std_msgs.Bool
        {
            data = LeftprimaryButtonValue
        };
        Debug.Log("LeftprimaryButtonValue" + LeftprimaryButtonValue);
        if (LeftprimaryButtonValue == true && flag_hide ==0 )
        {
            this.GetComponent<MeshRenderer>().enabled = !this.GetComponent<MeshRenderer>().enabled;
            flag_hide = 1;
        }
        else if (LeftprimaryButtonValue == false && flag_hide == 1)
        {
            flag_hide = 0;
        }

        //------------------Pub_Secondary Buttom------------------------------//

        RightsecondaryButtonValue = controllerInput.getRightSecondaryButton();
        std_msgs.Bool message_s = new std_msgs.Bool
        {
            data = RightsecondaryButtonValue
        };


        LeftsecondaryButtonValue = controllerInput.getLeftSecondaryButton();
        std_msgs.Bool message_s_l = new std_msgs.Bool
        {
            data = LeftsecondaryButtonValue
        };


    }
}
