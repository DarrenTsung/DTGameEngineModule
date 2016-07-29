using DT;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public abstract class GameSession<T> where T : class {
    public class GameSessionEvent : UnityEvent<T> {}

    // PRAGMA MARK - Public Interface
    [HideInInspector] public GameSessionEvent OnStarted = new GameSessionEvent();
    [HideInInspector] public GameSessionEvent OnFinished = new GameSessionEvent();

    public void Start() {
      if (this.Started) {
        Debug.LogWarning("GameSession.Start(): called when game session already started!");
        return;
      }

      this._started = true;
      this.HandleStart();
      this.OnStarted.Invoke(this as T);
    }

    public void Finish() {
      if (this.Finished) {
        Debug.LogWarning("GameSession.Finish(): called when game session already finished!");
        return;
      }

      this._finished = true;
      this.HandleFinish();
      this.OnFinished.Invoke(this as T);
    }

    public bool Started {
      get {
        return this._started;
      }
    }

    public bool Finished {
      get {
        return this._finished;
      }
    }


    // PRAGMA MARK - Internal
    [SerializeField, ReadOnly]
    private bool _started;
    [SerializeField, ReadOnly]
    private bool _finished;

    protected abstract void HandleStart();
    protected abstract void HandleFinish();
  }
}