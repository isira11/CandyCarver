using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineRotator : MonoBehaviour
{

    public float speed = 20;

    public void Update()
    {
        transform.localEulerAngles = new Vector3(0,0, transform.localEulerAngles.z + Time.deltaTime * speed); 
    }
}
 