using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down")]
    public float _throttleIncreament = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    public float _maxThrust = 200f;
    [Tooltip("How responsive the Aircraft is when rolling, pithing, and yawing")]
    public float _responsiveness = 10f;

    public float throttle;
    public float yaw;
    public float roll;
    public float pitch;

    private float responseModifier
    {
        get{
            return (rb.mass / 10f) * _responsiveness;
        }
    }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
            throttle += _throttleIncreament;
        else if (Input.GetKey(KeyCode.LeftControl))
            throttle -= _throttleIncreament;
    }

    private void Update()
    {
        HandleInputs();
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * _maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.up * roll * responseModifier);
    }
}
