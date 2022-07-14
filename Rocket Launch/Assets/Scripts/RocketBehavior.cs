using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    Rigidbody _rb;
    AudioSource _as;
    // Start is called before the first frame update
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
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddRelativeForce(Vector3.up);
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
        _rb.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            transform.Rotate(Vector3.back);
        }
        _rb.freezeRotation = false;
    }
}
