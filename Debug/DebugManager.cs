using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugManager : MonoBehaviour {
    public bool IsDebug {
      get {
        return Debug.isDebugBuild;
      }
    }

    [SerializeField]
    private bool _showFps = true;
    private FPSViewController _fpsViewController;

    private void Awake() {
      this._fpsViewController = new FPSViewController();
      this.RefreshShowingViewControllers();
    }

    private void OnValidate() {
      this.RefreshShowingViewControllers();
    }

    private void RefreshShowingViewControllers() {
      if (this._fpsViewController != null) {
        this.DismissOrShowViewController(this._showFps, this._fpsViewController);
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