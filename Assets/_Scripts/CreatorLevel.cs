using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorLevel : Singleton<CreatorLevel>
{
    /// <summary>
    /// List of premade patterns of bricks positions.
    /// </summary>
    [Tooltip("List of premade patterns of bricks positions")]
    public List<GameObject> patternList;

    /// <summary>
    /// Brick GameObject.
    /// </summary>
    [Tooltip("Brick GameObject")]
    public GameObject brick;

    /// <summary>
    /// List of in game bricks.
    /// </summary>
    private List<GameObject> bricks = new List<GameObject>();

    /// <summary>
    /// Catch positions for bricks in a pattern and instantiate a brick with random level in each.
    /// </summary>
    /// <param name="currentLevel">Current level of the game, max level of instantiate bricks</param>
    public void CreateLevel(int currentLevel)
    {
        List<Transform> positions = new List<Transform>();
        //TO DO: select a random pattern
        for ( int i = 0; i < patternList[0].gameObject.transform.childCount; i++)
        {
            positions.Add(patternList[0].gameObject.transform.GetChild(i).transform);
        }        
        GameObject instance;
        foreach (Transform pos in positions)
        {
            instance = Instantiate(brick, pos.position, brick.transform.rotation);
            bricks.Add(instance);
            int level = Random.Range(1, currentLevel + 1);
            instance.GetComponent<BrickBehaviour>().IniatiateBrick(level);
        }
        positions.Clear();
    }

    /// <summary>
    /// Remove a destroyed brick from in game bricks list.
    /// </summary>
    /// <param name="brick">Brick to remove</param>
    public void EliminateBrick(GameObject brick)
    {
        bricks.Remove(brick);
    }

    /// <summary>
    /// Remove the remains brick from the ingame bricks list.
    /// </summary>
    public void ClearLevel()
    {
        for (int i = bricks.Count - 1; i > -1 ; i--)
        {
            Destroy(bricks[i]);
            bricks.RemoveAt(i);
        }
    }
}
