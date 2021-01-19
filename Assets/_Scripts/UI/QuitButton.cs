using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    /// <summary>
    /// Button component of the GameObject.
    /// </summary>
    private Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(QuitGame);
    }

    /// <summary>
    ///  Invoke quit game event of GameManager.
    /// </summary>
    private void QuitGame()
    {
        GameManager.Instance.eventQuitGame.Invoke();
    }
}
