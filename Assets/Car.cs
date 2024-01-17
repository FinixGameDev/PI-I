using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float Speed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.forward * Time.deltaTime * Speed;
    }
}
