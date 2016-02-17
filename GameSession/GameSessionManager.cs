using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class GameSessionManager<T> : MonoBehaviour where T : GameSession<T> {
    // PRAGMA MARK - Public Interface
    public T ActiveGameSession {
      get; private set;
    }

    // @return the game session started
    public void StartGameSession(T gameSession) {
      if (gameSession.Started) {
        Debug.LogWarning("StartGameSession: failed because game session already started!");
        return;
      }

      gameSession.OnFinished.AddListener(this.HandleGameSessionFinished);

      this.ActiveGameSession = gameSession;
      this.ActiveGameSession.Start();
      DTGameEngineNotifications.GameSessionStarted.Invoke(gameSession);
    }


    // PRAGMA MARK - Internal
    protected void HandleGameSessionFinished(T finishedGameSession) {
      finishedGameSession.OnFinished.RemoveListener(this.HandleGameSessionFinished);
      if (finishedGameSession == this.ActiveGameSession) {
        this.ActiveGameSession = null;
        this.ActiveGameSessionWasFinished(finishedGameSession);
      }
    }

    protected virtual void ActiveGameSessionWasFinished(T finishedGameSession) {
      DTGameEngineNotifications.GameSessionFinished.Invoke(finishedGameSession);
    }
  }
}