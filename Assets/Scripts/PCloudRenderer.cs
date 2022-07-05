using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class PCloudRenderer : UnitySubscriber<MessageTypes.Sensor.PointCloud2>
    {
        public int LidarLayer = 1;
        public float LidarHeight = 0.2f;

        private byte[] byteArray;
        private bool isMessageReceived = false;
        private int size;

        private Vector3[] pcl;
        private Color[] pcl_color;

        int width;
        int height;
        int row_step;
        int point_step;

        public Transform offset; // Put any gameobject that faciliatates adjusting the origin of the pointcloud in VR. 

 
        private int sizelayer;
        private int topic; //1: velodyne, 2: laser scan

        Mesh mesh;
        MeshRenderer pclRenderer;
        MeshFilter mf;
        public float pointSize = 10f;

        [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
        private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
        private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };

        protected override void Start()
        {
            base.Start();
            Debug.Log("ROS_PCloud start");
            // Debug.Log("ROS_PCloud start");
            pclRenderer = gameObject.AddComponent<MeshRenderer>();
            mf = gameObject.AddComponent<MeshFilter>();
            pclRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
            mesh = new Mesh
            {
                // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
                indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
            };
        }

        public void Update()
        {

            if (isMessageReceived)
            {
                // Debug.Log("ROS_PCloud Get");
                // Debug.Log(byteArray[33]);
                PointCloudRendering();
                transform.position = offset.position;
                transform.rotation = offset.rotation;
                pclRenderer.material.SetFloat("_PointSize", pointSize);
                UpdateMesh();

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

            size = byteArray.GetLength(0);

            point_step = 16; //scan_pcl 16,      32; // velodyne
            size = size / point_step;
            sizelayer = size * LidarLayer;

            pcl = new Vector3[sizelayer];
            pcl_color = new Color[sizelayer];

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

            for (int w = 0; w < LidarLayer; w++)
            {
                for (int n = 0; n < size; n++)
                {
                    x_posi = n * point_step + 0;
                    y_posi = n * point_step + 4;
                    z_posi = n * point_step + 8;

                    y = BitConverter.ToSingle(byteArray, x_posi);
                    x = BitConverter.ToSingle(byteArray, y_posi);
                    z = BitConverter.ToSingle(byteArray, z_posi);

                    z = z + w * LidarHeight;

                    pcl[w * size + n] = new Vector3(-x, z, y);

                    rgb_posi = n * point_step + 8; // 16   8: laser scan

                    b = byteArray[rgb_posi + 0];
                    g = byteArray[rgb_posi + 1];
                    r = byteArray[rgb_posi + 2];

                    r = r / rgb_max;
                    g = g / rgb_max;
                    b = b / rgb_max;

                    pcl_color[w * size + n] = new Color(r, g, b);
                }
            }

        }

        void UpdateMesh()
        {

            positions = pcl;
            colours = pcl_color;
            if (positions == null)
            {
                return;
            }
            mesh.Clear();
            mesh.vertices = positions;
            mesh.colors = colours;
            int[] indices = new int[positions.Length];

            for (int i = 0; i < positions.Length; i++)
            {
                indices[i] = i;
            }

            mesh.SetIndices(indices, MeshTopology.Points, 0);
            mf.mesh = mesh;

        }
    }
}
