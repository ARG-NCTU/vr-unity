using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCube : MonoBehaviour
{

    public ControllersManager inputs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.getPrimaryButton())
        {
            this.TeleportRandomly();
        }
    }

    private void TeleportRandomly()
    {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1.0f);
        float distance = 2.0f * Random.value + 1.5f;
        transform.localPosition = distance * direction;
    }
}
