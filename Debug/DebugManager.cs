using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugManager : MonoBehaviour {
    // PRAGMA MARK - Static
    public static bool IsDebug {
      get {
        return Debug.isDebugBuild;
      }
    }


    // PRAGMA MARK - Internal
    private FPSViewController _fpsViewController;
    private GameObject _debugView;
    private int _previousNumberOfTouches;

    void Awake() {
      if (!DebugManager.IsDebug) {
        this.enabled = false;
        return;
      }

      DebugLoggerView.Initialize();

      this._fpsViewController = new FPSViewController();
      this._fpsViewController.Show();

      this._debugView = ObjectPoolManager.Instantiate("DebugView");
      ViewManagerLocator.Main.AttachView(this._debugView);
      this._debugView.SetActive(false);
    }

    void Update() {
      if (!DebugManager.IsDebug) {
        return;
      }

      int numberOfTouches = Input.touches.Length;
      if (numberOfTouches != this._previousNumberOfTouches) {
        if (Input.touches.Length == 3) {
          this._debugView.ToggleActive();
        }
      }

      if (Input.GetKeyDown(KeyCode.D)) {
        this._debugView.ToggleActive();
      }

      this._previousNumberOfTouches = numberOfTouches;
    }
  }
}