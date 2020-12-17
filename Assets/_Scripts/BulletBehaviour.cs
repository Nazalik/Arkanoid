using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    /// <summary>
    /// Bullet speed.
    /// </summary>
    [SerializeField, Range(5, 20), Tooltip("Bullet speed")]
    private float speed = 10;
    
    // Update method.
    // Move the bullet up.
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);    
    }

    // OnCollisionEnter Method.
    // Destoy the object when hit a brick.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(gameObject);
        }
    }

    // OnTriggerEnter Method.
    // Destroy the object when enter a brick trigger (bulldozer effect) or killzone area.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("KillZone"))
        {
            Destroy(gameObject);
        }
    }
}
