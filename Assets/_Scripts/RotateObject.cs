using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    /// <summary>
    /// Rigidbody of the GameObject.
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Rotation force.
    /// </summary>
    [SerializeField, Tooltip("Rotation force")]
    private float torqueForce = 10f;

    // Start method.
    // Catch rigidbody and assign a torque force.
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
    }

    /// <summary>
    /// Generate a random float.
    /// </summary>
    /// <returns>Random float</returns>
    private float RandomTorque()
    {
        return Random.Range(-torqueForce, torqueForce);
    }
}
