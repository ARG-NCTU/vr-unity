using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sensors : MonoBehaviour
{
    
    private PhotonView photonView;
    
    
    //RPC_Audio
    private float[] LeftChannel = new float[16000]; 
    
    //RPC_LiDAR
    private byte[] byteArray;

    int point_step;
    private int size;
    private int sizelayer;
    public int LidarLayer = 1;
    public float LidarHeight = 0.2f;
    private int topic; //1: velodyne, 2: laser scan
    private Vector3[] pcl;
    private Color[] pcl_color;

    Mesh mesh;
    MeshRenderer pclRenderer;
    MeshFilter mf;
    public float pointSize = 10f;

    //RPC_image
    private byte[] imageData;
    private Texture2D texture2D;
    public MeshRenderer imageRenderer;


    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };

    public Transform offset; // Put any gameobject that faciliatates adjusting the origin of the pointcloud in VR. 

    // --------------------------------
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        // -----------RPC_LiDAR---------------
        // Give all the required components to the gameObject
        pclRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        pclRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh
        {
            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        transform.position = offset.position;
        transform.rotation = offset.rotation;
        // -----------RPC_LiDAR---------------

        // -----------RPC_Image---------------
        texture2D = new Texture2D(1, 1);
        imageRenderer.material = new Material(Shader.Find("Standard"));
        // -----------RPC_Image---------------

    }

    // ================================================
    [PunRPC]
    public void RPC_Audio(float[] LeftChannel)
    {
        // Debug.Log("RPC_Audio");
        this.LeftChannel = LeftChannel;
        // Debug.Log(LeftChannel[5]);
        // AudioClip audioClip = AudioClip.Create("WavFileSound", 16000, 2, 8000, false);
        // audioClip.SetData(this.LeftChannel, 0);
        // AudioSource audio = GetComponent<AudioSource>();
        // audio.clip = audioClip;
        // audio.Play();
        
    }

    // ===================== RPC_LiDAR: Start ===========================
    [PunRPC]
    public void RPC_LiDAR(byte[] byteArray)
    {
        // Debug.Log("RPC_LiDAR");
        // Debug.Log(byteArray[33]);
        this.byteArray = byteArray;
        PointCloudRendering(byteArray);
        transform.position = offset.position;
        transform.rotation = offset.rotation;
        pclRenderer.material.SetFloat("_PointSize", pointSize);       
        UpdateMesh();
    }

    void PointCloudRendering(byte[] byteArray)
    {
        
        size = byteArray.GetLength(0);

        point_step = 16; //scan_pcl 16,      32; // velodyne
        size = size / point_step;
        sizelayer = size*LidarLayer;

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

                z = z + w*LidarHeight;

                pcl[w*size+n] = new Vector3(-x, z, y);

                rgb_posi = n * point_step + 8; // 16   8: laser scan

                b = byteArray[rgb_posi + 0];
                g = byteArray[rgb_posi + 1];
                r = byteArray[rgb_posi + 2];

                r = r / rgb_max;
                g = g / rgb_max;
                b = b / rgb_max;

                pcl_color[w*size+n] = new Color(r, g, b);
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
    // ================ RPC_LiDAR: End ================================
     // ===================== RPC_Image ===========================
    
    [PunRPC]
    public void RPC_Image(byte[] imageData)
    {	
        new WaitForSeconds(1);
        // Debug.Log("RPC_image");
        // Debug.Log(imageData[7]);
        this.imageData = imageData;
        texture2D.LoadImage(this.imageData);
        texture2D.Apply();
        imageRenderer.material.SetTexture("_MainTex", texture2D);
    }


    // ================================================
    
}
