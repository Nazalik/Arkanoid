using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : Singleton<PowerUpSpawner>
{
    /// <summary>
    /// List of possible power ups.
    /// </summary>
    [Tooltip("List of possible power ups. the amount. The number of occurrences influences the probability of spawn")]
    public List<GameObject> powerUpPrefabs;

    /// <summary>
    /// Spawn a random power up in a given position with a probability relative to game level.
    /// </summary>
    /// <param name="position">Position where spawn the power up</param>
    public void GeneratePowerUp(Vector3 position)
    {
        float spawnPowerUpFactor = Random.Range(0, 99);
        if (spawnPowerUpFactor < GameManager.Instance.GetLevel())
        {
            int SpawnElement = Random.Range(0, powerUpPrefabs.Count);
            Instantiate(powerUpPrefabs[SpawnElement], position, powerUpPrefabs[0].transform.rotation);
        }
    }
}
