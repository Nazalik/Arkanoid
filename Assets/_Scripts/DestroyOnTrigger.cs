using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    /// <summary>
    /// Tag to look for.
    /// </summary>
    [Tooltip("Tag to look for")]
    public string lookForTag = "";

    // OnTriggerEnter Method
    // Destroy the gameObject if the trigger area tag is equal to loof for tag.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(lookForTag))
        {
            Destroy(gameObject);
        }
    }
}
