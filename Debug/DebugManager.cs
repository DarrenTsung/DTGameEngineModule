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

    private void Awake() {
      this._fpsViewController = new FPSViewController();
      this.RefreshShowingViewControllers();
    }

    private void RefreshShowingViewControllers() {
      if (this._fpsViewController != null) {
        this.DismissOrShowViewController(DebugManager.IsDebug, this._fpsViewController);
      }
    }

    private void DismissOrShowViewController(bool show, IViewController viewController) {
      if (show) {
        viewController.Show();
      } else {
        viewController.Dismiss();
      }
    }
  }
}