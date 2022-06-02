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
    public class PointCloudPun : UnitySubscriber<MessageTypes.Sensor.PointCloud2>
    {
        public GameObject feedbackInput;
        private PhotonView feedback;    // Start is called before the first frame update
        private byte[] byteArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        private int size;
        private Vector3[] pcl;
        private Color[] pcl_color;

        int width;
        int height;
        int row_step;
        int point_step;

        protected override void Start()
        {
            base.Start();
            Debug.Log("ROS_PCloud start");
            feedback = feedbackInput.GetComponent<PhotonView>();
            // Debug.Log("ROS_PCloud start");
        }

        public void Update()
        {

            if (isMessageReceived)
            {
                // Debug.Log("ROS_PCloud Get");
                // Debug.Log(byteArray[33]);
                // PointCloudRendering();
                feedback.RPC("RPC_LiDAR", RpcTarget.All, byteArray);
                isMessageReceived = false;
            }

        }

        protected override void ReceiveMessage(PointCloud2 message)
        {
            
            size = message.data.GetLength(0);

            byteArray = new byte[size];
            byteArray = message.data;


            width = (int)message.width;
            height = (int)message.height;
            row_step = (int)message.row_step;
            point_step = (int)message.point_step;
            size = size / point_step;
            isMessageReceived = true;
        }

        void PointCloudRendering()
        {
            pcl = new Vector3[size];
            pcl_color = new Color[size];

            int x_posi;
            int y_posi;
            int z_posi;

            float x;
            float y;
            float z;

            int rgb_posi;
            int rgb_max = 255;

            float r;
            float g;
            float b;

            //Debug.Log("ROS_PCloud Get");
            //Debug.Log(pcl[5]);
        
            for (int n = 0; n < size; n++)
            {
                x_posi = n * point_step + 0;
                y_posi = n * point_step + 4;
                z_posi = n * point_step + 8;

                x = BitConverter.ToSingle(byteArray, x_posi);
                y = BitConverter.ToSingle(byteArray, y_posi);
                z = BitConverter.ToSingle(byteArray, z_posi);


                rgb_posi = n * point_step + 16;

                b = byteArray[rgb_posi + 0];
                g = byteArray[rgb_posi + 1];
                r = byteArray[rgb_posi + 2];

                r = r / rgb_max;
                g = g / rgb_max;
                b = b / rgb_max;

                pcl[n] = new Vector3(x, z, y);
                pcl_color[n] = new Color(r, g, b);

            }
        }

        public Vector3[] GetPCL()
        {
            return pcl;
        }

        public Color[] GetPCLColor()
        {
            return pcl_color;
        }
    }
}
