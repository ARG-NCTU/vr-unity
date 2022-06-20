using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] private Material SetectedMaterial;
    [SerializeField] private Material DefaultMaterial;
    //private Material DefaultMaterial = null;
    private Renderer objectRenderer = null;

    RaycastHit hit;
    private Transform _selection = null;
    public Transform rightController;
    
    void Update()
    {
        if ( _selection != null)
        {
            if (objectRenderer == null)
            {
                objectRenderer = _selection.GetComponent<Renderer>();   
            }

            if(objectRenderer != null && DefaultMaterial != null)
            {
                objectRenderer.material = DefaultMaterial;
            }
            _selection = null;
        }

        Ray ray = new Ray(rightController.position, rightController.forward);
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.tag == "target")
            {
                var selection = hit.transform;
                objectRenderer = selection.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    if(DefaultMaterial == null)
                    {
                        DefaultMaterial = objectRenderer.material;
                    }
                    objectRenderer.material = SetectedMaterial;
                }
                float distance = hit.distance;
                //Debug.Log(distance); // Distance from the controller_transfrom.forward
                Transform transform_f_controll = hit.transform;
                Debug.Log(transform_f_controll.position.ToString()); // hit_Object_global_transform
                _selection = selection;
            }
                
        }

    }
}
