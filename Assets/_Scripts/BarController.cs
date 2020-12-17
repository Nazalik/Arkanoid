#if UNITY_IOS || UNITY_ANDROID
    #define USING_MOBILE
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to control the behaviour of the player bar.
/// </summary>
public class BarController : MonoBehaviour
{
    /// <summary>
    /// Movement speed.
    /// </summary>
    [SerializeField, Range(5, 20), Tooltip("Movement speed")]
    private float moveSpeed = 5;

    private float xBoundary = 7.8f;

    /// <summary>
    /// Scale factor up when catch a size up power up.
    /// </summary>
    private Vector3 powerUpSizeUp = new Vector3(0, 0.25f, 0);

    /// <summary>
    /// Maximum scale that can be reach with size up power up.
    /// </summary>
    private float maxScale = 2.5f;

    /// <summary>
    /// Increment of speed when catch a speed up power up.
    /// </summary>
    private float powerUpSpeedUp = 1;

    /// <summary>
    /// Maximum movement speed that can be reach with speed up power up.
    /// </summary>
    private float maxSpeed = 25f;

    /// <summary>
    /// Bullet GameObject to fire when catch a gun power up.
    /// </summary>
    [Tooltip("Bullet GameObject to fire when catch a gun power up")]
    public GameObject bullet;

    /// <summary>
    /// Duration of gun power up.
    /// </summary>
    [SerializeField, Range(1, 10), Tooltip("Duration of gun power up")]
    private float railgunTime = 5f;

    /// <summary>
    /// Number of bullets to fire in gun power up mode.
    /// </summary>
    [SerializeField, Range(1, 10), Tooltip("Number of bullets to fire in gun power up mode")]
    private int nBullets = 5;
    
    // Start Method.
    void Start()
    {
        
    }

    // Update Method.
    // move the bar according to the input on horizontal axis. Check bar position to maintance it within limits.
    void Update()
    {
#if USING_MOBILE
        float hmove = Input.GetAxis("Mouse X");

        if (Input.touchCount > 0)
        {
            hmove = Input.touches[0].deltaPosition.x;
        }
#else
        float hmove = Input.GetAxis("Horizontal");
    #endif

        float movement = hmove * moveSpeed * Time.deltaTime;
        Vector3 mov = new Vector3(hmove * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(mov,Space.World);
        KeepInBounds();
    }

    // OnTriggerEnter Method.
    // Check when the bar enter in a trigger area (usually powerups) and proceeds according to the type of power up
    private void OnTriggerEnter(Collider other)
    {
        // Size Up.
        // Check if scale is below the limit, add the increment and destroy the power up.
        if(other.gameObject.CompareTag("PowerUpSizeUp"))
        {
            if(gameObject.transform.localScale.y < maxScale)
            {
                gameObject.transform.localScale += powerUpSizeUp;
            }            
            Destroy(other.gameObject);
        }
        // Spped Up.
        // Increase speed if it is below the limit and destroy the power up.
        else if (other.gameObject.CompareTag("PowerUpSpeedUp"))
        {
            if (moveSpeed < maxSpeed)
            {
                moveSpeed += powerUpSpeedUp;
            }
            Destroy(other.gameObject);
        }
        // Bulldozer.
        // Inform the GameManager to apply the bulldozer effect and destroy the power up.
        else if (other.gameObject.CompareTag("PowerUpBulldozer"))
        {
            GameManager.Instance.eventPowerUpBulldozer.Invoke();
            Destroy(other.gameObject);
        }
        // Add Ball.
        // Inform the GameManager to add a new ball and destroy the power up. 
        else if (other.gameObject.CompareTag("PowerUpAddBall"))
        {
            GameManager.Instance.eventPowerUpAddBall.Invoke(other.gameObject);
            Destroy(other.gameObject);
        }
        // add Life.
        // Inform the GameManager to add a new life an destroy the power up.
        else if (other.gameObject.CompareTag("PowerUpAddLife"))
        {
            GameManager.Instance.eventPowerUpAddLive.Invoke();
            Destroy(other.gameObject);
        }
        // Gun.
        // Start the coroutine to fire five bullets and destroy the power up.
        else if (other.gameObject.CompareTag("PowerUpGun"))
        {
            StartCoroutine(Railgun());
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// keeps the bar within limits
    /// </summary>
    private void KeepInBounds()
    {
        if (transform.position.x > xBoundary)
        {
            transform.position = new Vector3(xBoundary, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -xBoundary)
        {
            transform.position = new Vector3(-xBoundary, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Coroutine to fire n bullets within a indicated time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Railgun()
    {
        int counter = 0;
        while (counter < nBullets)
        {
            Instantiate(bullet, gameObject.transform.position, bullet.transform.rotation);
            counter++;
            yield return new WaitForSeconds(railgunTime / nBullets);
        }
        yield break;
    }
}
