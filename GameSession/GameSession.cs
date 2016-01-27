using DT;
using UnityEngine;

namespace DT.GameEngine {
  public abstract class GameSession<T> : MonoBehaviour where T : class {
    // PRAGMA MARK - Public Interface
    public delegate void HandleGameSessionFinished(T finishedGameSession);
    public event HandleGameSessionFinished OnGameSessionFinished = delegate {};

    [SerializeField, ReadOnly]
    private bool _isFinished;
    public bool IsFinished {
      get {
        return this._isFinished;
      }
      protected set {
        bool oldValue = this._isFinished;
        this._isFinished = value;
        if (!oldValue && this._isFinished) {
          this.Cleanup();
          this.OnGameSessionFinished.Invoke(this as T);
        }
      }
    }


    // PRAGMA MARK - Internal
    protected void Awake() {
      this.Initialize();
    }

    protected abstract void Initialize();
    protected abstract void Cleanup();
  }
}