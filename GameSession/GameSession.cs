using DT;
using UnityEngine;

namespace DT.Game {
  public class GameSession : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public delegate void HandleGameSessionFinished(GameSession finishedGameSession);
    public event HandleGameSessionFinished OnGameSessionFinished = delegate {};

    [SerializeField, ReadOnly]
    private bool _isFinished;
    public bool IsFinished {
      get {
        return this._isFinished;
      }
      private set {
        this._isFinished = value;
        if (this._isFinished) {
          this.OnGameSessionFinished.Invoke(this);
        }
      }
    }


    // PRAGMA MARK - Internal
  }
}