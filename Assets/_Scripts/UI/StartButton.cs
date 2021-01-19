using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    /// <summary>
    /// Button component of the GameObject.
    /// </summary>
    private Button _button;

    // Start method.
    // Catch button component and add listener.
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartGame);
    }

    /// <summary>
    /// Invoke start game event of GameManager.
    /// </summary>
    private void StartGame()
    {
        GameManager.Instance.eventStartGame.Invoke();
    }
}
