using DT;
using UnityEngine;

namespace DT.Game {
  public class GameSessionManager : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public GameSession ActiveGameSession {
      get; private set;
    }
    
    // @return the game session started
    public GameSession StartNewGameSession() {
      GameObject sessionGameObject = new GameObject("GameSession");
      sessionGameObject.transform.SetParent(this.transform);
      
      GameSession newGameSession = sessionGameObject.AddComponent<GameSession>();
      newGameSession.OnGameSessionFinished += this.HandleGameSessionFinished;
      this.ActiveGameSession = newGameSession;
      
      return newGameSession;
    }
    
    
    // PRAGMA MARK - Internal
    protected void HandleGameSessionFinished(GameSession finishedGameSession) {
      finishedGameSession.OnGameSessionFinished -= this.HandleGameSessionFinished;
      if (finishedGameSession == this.ActiveGameSession) {
        this.ActiveGameSession = null;
      }
    }
  }
}