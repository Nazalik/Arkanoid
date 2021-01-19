using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
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
        _button.onClick.AddListener(ExitGame);
    }

    /// <summary>
    /// Close app.
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();
    }
}
