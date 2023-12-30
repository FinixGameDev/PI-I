using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _target.position;
    }

    // LateUpdate is called after Update
    void FixedUpdate()
    {
        Vector3 newPostion = new Vector3(transform.position.x, transform.position.y, _offset.z + _target.position.z);
        transform.position = Vector3.Lerp(transform.position, newPostion, 10 * Time.fixedDeltaTime);
    }
}
