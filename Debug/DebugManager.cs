using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugManager : Singleton<DebugManager> {
    // PRAGMA MARK - Static
    private const float kTouchScrubMaxDistance = 400.0f;
    private const float kTouchScrubMinDistance = 100.0f;
    private const float kTimeScaleScrubMax = 20.0f;
    private const float kTimeScaleScrubMin = 0.1f;

    public static bool IsDebug {
      get {
        return Debug.isDebugBuild;
      }
    }

    private static float? _timeOffset;
    public static float TimeOffset {
      get {
        if (DebugManager._timeOffset == null) {
          DebugManager._timeOffset = PlayerPrefs.GetFloat("DebugManager::TimeOffset", defaultValue: 0.0f);
        }
        return DebugManager._timeOffset.Value;
      }
      set {
        DebugManager._timeOffset = value;
        PlayerPrefs.SetFloat("DebugManager::TimeOffset", DebugManager._timeOffset.Value);
      }
    }


    // PRAGMA MARK - Internal
    private GameObject _debugMenu;

    void Awake() {
      if (!DebugManager.IsDebug) {
        this.enabled = false;
        return;
      }

      DebugLogger.Initialize();

      GameObject fpsView = ObjectPoolManager.Instantiate("FPSView");
      ViewManagerLocator.Main.AttachView(fpsView);

      GameObject timeScaleScrubView = ObjectPoolManager.Instantiate("TimeScaleScrubView");
      ViewManagerLocator.Main.AttachView(timeScaleScrubView);

      CoroutineWrapper.StartCoroutine(this.UpdateToggleDebugView());
      CoroutineWrapper.StartCoroutine(this.UpdateTimeScaleScrubbing());
    }

    private GameObject DebugMenu {
      get {
        if (this._debugMenu == null) {
          this._debugMenu = ObjectPoolManager.Instantiate("DebugMenu");
          ViewManagerLocator.Main.AttachView(this._debugMenu);
          this._debugMenu.SetActive(false);
        }

        return this._debugMenu;
      }
    }

    private IEnumerator UpdateToggleDebugView() {
      int previousNumberOfTouches = 0;

      while (true) {
        int numberOfTouches = Input.touches.Length;
        if (numberOfTouches != previousNumberOfTouches) {
          if (Input.touches.Length == 3) {
            this.DebugMenu.ToggleActive();
          }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
          this.DebugMenu.ToggleActive();
        }

        previousNumberOfTouches = numberOfTouches;
        yield return null;
      }
    }

    private IEnumerator UpdateTimeScaleScrubbing() {
      int previousNumberOfTouches = 0;
      Vector2 scrubTouchStartingCenter = Vector2.zero;

      while (true) {
        float timeScaleScrub = 0.0f;

        if (Input.GetKey(KeyCode.E)) {
          timeScaleScrub = 1.0f;
        } else if (Input.GetKey(KeyCode.Q)) {
          timeScaleScrub = -1.0f;
        }

        int numberOfTouches = Input.touches.Length;
        if (numberOfTouches == 2) {
          Vector2 touchCenter = Input.touches.Average(t => t.position);
          if (numberOfTouches != previousNumberOfTouches) {
            // start scrubbing
            scrubTouchStartingCenter = touchCenter;
          }

          float yOffset = touchCenter.y - scrubTouchStartingCenter.y;
          if (yOffset > 0.0f) {
            timeScaleScrub = Mathf.Min(yOffset, kTouchScrubMaxDistance) / kTouchScrubMaxDistance;
          } else if (yOffset < 0.0f) {
            timeScaleScrub = Mathf.Max(yOffset, -kTouchScrubMinDistance) / kTouchScrubMinDistance;
          }
        }

        float timeScale = 1.0f;
        if (timeScaleScrub > 0) {
          timeScale = Mathf.Lerp(1.0f, kTimeScaleScrubMax, timeScaleScrub);
        } else if (timeScaleScrub < 0) {
          timeScale = Mathf.Lerp(1.0f, kTimeScaleScrubMin, -timeScaleScrub);
        }

        Time.timeScale = timeScale;

        float timeScaleDifference = timeScale - 1.0f;
        DebugManager.TimeOffset += Time.unscaledDeltaTime * timeScaleDifference;

        previousNumberOfTouches = numberOfTouches;
        yield return null;
      }
    }
  }
}