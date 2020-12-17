using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehaviour : MonoBehaviour
{
    /// <summary>
    /// Collider of the gameObject.
    /// </summary>
    private BoxCollider _collider;

    /// <summary>
    /// List of materials for the differents levels.
    /// </summary>
    [SerializeField, Tooltip("List of materials for the differents levels")]
    private List<Material> materials;

    /// <summary>
    /// level wich had at the instantiate moment.
    /// </summary>
    private int initialLevel;

    /// <summary>
    /// Current level of the brick.
    /// </summary>
    private int brickLevel;

    /// <summary>
    /// number of remains hits to destroy the brick.
    /// </summary>
    private int hits = 1;

    /// <summary>
    /// Used to obtain the point value related with initial level.
    /// </summary>
    [SerializeField, Tooltip("Used to obtain the point value related with initial level")]
    private int pointValueMultiplier = 10;

    /// <summary>
    /// Point value obtained when destroy the brick.
    /// </summary>
    private int pointValue;

    /// <summary>
    /// Duration of the bulldozer effect.
    /// </summary>
    private float bulldozerEffectDuration = 5f;

    // Start Method.
    // Catch collider of the object and create listener to Bulldozer event of GameManager.
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        GameManager.Instance.eventPowerUpBulldozer.AddListener(ActiveBulldozer);
    }

    /// <summary>
    /// Give initial level and establish point value.
    /// </summary>
    /// <param name="level">Initial level</param>
    public void IniatiateBrick(int level)
    {
        initialLevel = level;
        UpdateLevel(initialLevel);
        StablishePointValue();
    }

    /// <summary>
    /// Update the current level, number of hits to destoy and current material.
    /// </summary>
    /// <param name="level">Current Level</param>
    public void UpdateLevel(int level)
    {
        brickLevel = level;
        hits = level;
        gameObject.GetComponent<Renderer>().material = materials[Mathf.Clamp(brickLevel - 1, 0, materials.Count - 1)];
    }

    /// <summary>
    /// Calculate the point value, multiplying initial level by value point multiplier
    /// </summary>
    private void StablishePointValue()
    {
        pointValue = initialLevel * pointValueMultiplier;
    }

    /// <summary>
    /// Active Bulldozer effect
    /// </summary>
    private void ActiveBulldozer()
    {
        StartCoroutine(BulldozerPowerUp());
    }

    /// <summary>
    /// Change collider to trigger for n seconds.
    /// </summary>
    private IEnumerator BulldozerPowerUp()
    {
        _collider.isTrigger = true;
        yield return new WaitForSeconds(bulldozerEffectDuration);
        _collider.isTrigger = false;
    }

    // OnCollisionEnter Method
    // if hit by the ball or bullet call the method HitBrick.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" || collision.gameObject.CompareTag("Bullet"))
        {
            HitBrick();
        }        
    }

    // OnTriggerEnter Method.
    // if hit by the ball or bullet destory object.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") || other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// reduce current level and check if is necessary destroy the object.
    /// </summary>
    public void HitBrick()
    {
        brickLevel--;
        if(brickLevel> 0)
        {
            UpdateLevel(brickLevel);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Inform GameManager that this brick has been destroyed, sending the point value and position.
    /// </summary>
    private void OnDestroy()
    {
        GameManager.Instance.eventBlockDestroyed.Invoke(gameObject, pointValue);
    }
}
