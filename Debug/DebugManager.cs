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
    private GameObject _debugLogger;
    private int _previousNumberOfTouches;

    void Awake() {
      if (!DebugManager.IsDebug) {
        this.enabled = false;
        return;
      }

      DebugLoggerView.Initialize();

      this._fpsViewController = new FPSViewController();
      this._fpsViewController.Show();

      this._debugLogger = ObjectPoolManager.Instantiate("DebugLoggerView");
      CanvasUtil.ParentUIElementToCanvas(this._debugLogger, CanvasUtil.ScreenSpaceMainCanvas);
      this._debugLogger.SetActive(false);
    }

    void Update() {
      if (!DebugManager.IsDebug) {
        return;
      }

      int numberOfTouches = Input.touches.Length;
      if (numberOfTouches != this._previousNumberOfTouches) {
        if (Input.touches.Length == 3) {
          this._debugLogger.ToggleActive();
        }
      }

      this._previousNumberOfTouches = numberOfTouches;
    }
  }
}