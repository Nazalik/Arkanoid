using UnityEngine.Events;
using UnityEngine;

public class Events
{
    /// <summary>
    /// Event to start the game.
    /// </summary>
    [System.Serializable] public class EventStartGame : UnityEvent { }

    /// <summary>
    /// Event to notify the destruction of a brick.
    /// </summary>
    [System.Serializable] public class EventBlockDestroyed : UnityEvent<GameObject, int> { }

    /// <summary>
    /// Event to notify the destuction of a ball.
    /// </summary>
    [System.Serializable] public class EventBallDestroyed : UnityEvent<GameObject> { }

    /// <summary>
    /// Event to notify the change of game state.
    /// </summary>
    [System.Serializable] public class EventGameStateChanged : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    /// <summary>
    /// Event to reset game over state.
    /// </summary>
    [System.Serializable] public class EventExitGameOver : UnityEvent { }

    /// <summary>
    /// Event to notify the collect of bulldozer power up.
    /// </summary>
    [System.Serializable] public class EventPowerUpBulldozer : UnityEvent { }

    /// <summary>
    /// Event to notify the collect of AddBall power up.
    /// </summary>
    [System.Serializable] public class EventPowerAddBall : UnityEvent<GameObject> { }

    /// <summary>
    /// Event to notify the collect of AddLife power up.
    /// </summary>
    [System.Serializable] public class EventPowerAddLife : UnityEvent { }
}
