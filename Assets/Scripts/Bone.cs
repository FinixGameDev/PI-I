using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 20 * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
