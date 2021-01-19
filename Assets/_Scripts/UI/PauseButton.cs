using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    /// <summary>
    /// Button component of the GameObject.
    /// </summary>
    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(PauseGame);
    }

    /// <summary>
    /// Invoke pause game event of GameManager.
    /// </summary>    
    private void PauseGame()
    {
        GameManager.Instance.eventPauseGame.Invoke();
    }
}
