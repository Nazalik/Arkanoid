using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the behaviour of the balls.
/// Is necesary to have a Rigidbody attached to the gameobject.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    /// <summary>
    /// Rigidbody of the gameobject
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Force applied in each hit or when the ball slow down.
    /// </summary>
    [SerializeField, Range(10, 30),Tooltip("Force applied in each hit or when the ball slow down")]
    private float force = 20f;

    /// <summary>
    /// Minimum speed until apply a new bost. 
    /// </summary>
    [SerializeField, Range(1, 20), Tooltip("Minimum speed until apply a new bost")]
    private float minSpeed = 10f;    

    // Start Method.
    // Catch rigidbody of the gameobject and apply the initial impulse.
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.down * force, ForceMode.Impulse);
    }

    
    // Update Method.
    // Check if the ball has slowed down below a defined value in wich case give it a bost in its current direction.
    void Update()
    {
        if (_rigidbody.velocity.magnitude < minSpeed)
        {
            _rigidbody.AddForce(_rigidbody.velocity.normalized * force, ForceMode.VelocityChange);
        }
    }

    // OnCollisionEnter Method.
    // If the object with wich the ball has collided is the player Bar, calculate the direction between ball and bar and aplly a bost in that dirrection.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            Vector3 direcction = transform.position - collision.gameObject.transform.position;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(direcction.normalized * force, ForceMode.VelocityChange);
        }             
    }

    //OntriggerEnter Method.
    // If the ball enter in the KillZone trigger area we have to destroy the ball and inform the GameManager.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
            GameManager.Instance.eventBallDestroyed.Invoke(gameObject);
        }
    }
}
