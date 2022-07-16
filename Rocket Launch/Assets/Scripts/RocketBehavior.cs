using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    Rigidbody _rb;
    AudioSource _as;

    [SerializeField] float rotationSpeed = 70f;
    [SerializeField] float flySpeed = 50f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RocketLaunch();
        RocketRotation();
    }

    void RocketLaunch()
    {
        float flySpeedDelta = flySpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) 
        {
            _rb.AddRelativeForce(Vector3.up * flySpeedDelta);
            if (!_as.isPlaying)
                _as.Play();
        }
        else
        {
            _as.Pause();
        }
    }

    void RocketRotation()
    {
        float rotationSpeedDelta = rotationSpeed * Time.deltaTime; 
        _rb.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeedDelta);
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            transform.Rotate(Vector3.back * rotationSpeedDelta);
        }
        _rb.freezeRotation = false;
    }
}
