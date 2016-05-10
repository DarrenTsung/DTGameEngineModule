using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class PlayerModule : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public virtual void SetupWithContext(Player context) {
      this._contextPlayerIndex = context.PlayerIndex;
      this._contextGameObject = context.gameObject;

      this.InitializeAfterContext();
    }

    public virtual void InitializeAfterAllModulesAdded() {}


    // PRAGMA MARK - Unity Lifecycle
    protected void OnEnable() {
      if (this._contextGameObject != null) {
        this.RegisterNotifications();
      }
    }

    protected void OnDisable() {
      this.CleanupNotifications();
    }


    // PRAGMA MARK - Internal
    protected int _contextPlayerIndex;
    protected GameObject _contextGameObject;

    // TODO (darren): remove and call SetupWithContext after all modules have been added
    protected virtual void InitializeAfterContext() {
      this.RegisterNotifications();
    }

    protected virtual void RegisterNotifications() { }
    protected virtual void CleanupNotifications() { }
  }
}