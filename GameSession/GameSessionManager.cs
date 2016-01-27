using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class GameSessionManager<T> : MonoBehaviour where T : GameSession<T> {
    // PRAGMA MARK - Public Interface
    public T ActiveGameSession {
      get; private set;
    }

    // @return the game session started
    public T StartNewGameSession() {
      GameObject sessionGameObject = new GameObject("GameSession");
      sessionGameObject.transform.SetParent(this.transform);

      T newGameSession = sessionGameObject.AddComponent<T>();
      newGameSession.OnGameSessionFinished += this.HandleGameSessionFinished;
      this.ActiveGameSession = newGameSession;
      DTGameEngineNotifications.GameSessionStarted.Invoke(newGameSession);

      return newGameSession;
    }


    // PRAGMA MARK - Internal
    protected void HandleGameSessionFinished(T finishedGameSession) {
      finishedGameSession.OnGameSessionFinished -= this.HandleGameSessionFinished;
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