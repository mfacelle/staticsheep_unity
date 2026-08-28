using UnityEngine;

public class GameStateManager : MbSingleton<GameStateManager>
{
    #region Variable

    public enum GameState
    {
        Intro, // introductory state (likely only used for level 1 stage 1)
        Paused, // when input actions should be disabled, but level is active
        Running, // when actively playing a level and processing input
        StageEnd, // when a level end state is reached (clear or failure)
        LevelSelect // at level select screen (TODO using Paused might be fine?)
    }

    public GameState CurrentState {get; private set;}

    #endregion

    public override void Awake()
    {
        base.Awake();

        // default to running, in the event a scene is launched via the editor out of order
        CurrentState = GameState.Running;
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;

        // TODO consider some kind of observer pattern for when state changes?
        // might be overkill, though
    }


}
