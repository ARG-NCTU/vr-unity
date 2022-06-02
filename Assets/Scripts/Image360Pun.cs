using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Photon.Pun;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class Image360Pun : UnitySubscriber<MessageTypes.Sensor.CompressedImage>
    {
        public GameObject feedbackInput;
        private PhotonView feedback;    // Start is called before the first frame update
        public MeshRenderer meshRenderer;

        private Texture2D texture2D;
        private byte[] imageData;
        private bool isMessageReceived;

        protected override void Start()
        {
			base.Start();
            feedback = feedbackInput.GetComponent<PhotonView>();
            // texture2D = new Texture2D(1, 1);
            // meshRenderer.material = new Material(Shader.Find("Standard"));
        }
        private void Update()
        {
            if (isMessageReceived)
                // Debug.Log("ROS_Image360");
                feedback.RPC("RPC_Image", RpcTarget.All, imageData);
                // ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.CompressedImage compressedImage)
        {
            imageData = compressedImage.data;
            isMessageReceived = true;
            // Debug.Log("ROS_Image360");
            // Debug.Log(imageData[7]);
        }

        private void ProcessMessage()
        {
            texture2D.LoadImage(imageData);
            texture2D.Apply();
            meshRenderer.material.SetTexture("_MainTex", texture2D);
            isMessageReceived = false;
        }

    }
}

