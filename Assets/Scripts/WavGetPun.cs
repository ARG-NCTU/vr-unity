using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using Photon.Pun;

public class WavGetPun : MonoBehaviour
{

    RosSocket rosSocket;
    private string RosBridgeServerUrl; //IP address
    public string topic_name;
    private string audio_data;
    private bool isMessageReceived = false;
    public GameObject feedbackInput;
    private PhotonView feedback;    // Start is called before the first frame update
    private float[] LeftChannel = new float[16000];

    void Start()
    {
        RosBridgeServerUrl = "ws://111.70.9.53:9090"; //"ws://111.70.9.53:9090"
        // GameObject.FindGameObjectWithTag("x1").GetComponent<RosConnector>().RosBridgeServerUrl;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        // Debug.Log("Established connection with ros(WAV PLAYER)");
        feedback = feedbackInput.GetComponent<PhotonView>();
        audio_data = rosSocket.Subscribe<std_msgs.Float32MultiArray>(topic_name, data_process);
        Debug.Log("ROS_Audio");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("ROS_Audio Update");
        if(isMessageReceived)
        {
            Debug.Log("ROS_Audio Get");
            Debug.Log(LeftChannel[5]);
            feedback.RPC("RPC_Audio", RpcTarget.All, LeftChannel);
            isMessageReceived = false;
        }
    }

    private void data_process(std_msgs.Float32MultiArray message)
    {
        LeftChannel = message.data;
        isMessageReceived = true;
    }
}
