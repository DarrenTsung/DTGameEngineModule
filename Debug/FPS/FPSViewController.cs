using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class FPSViewController : BasicViewController<FPSView> {
    public FPSViewController() {
      this._viewPrefabName = "FPSView";
      UnityBehaviourManager.OnUpdate += this.HandleUpdate;
    }

    private int _frameCount = 0;
    private float _dt = 0.0f;
    private float _fps = 0.0f;
    private float _updateRate = 4.0f;  // 4 updates per sec.

    private void HandleUpdate() {
      this._frameCount++;
      this._dt += Time.deltaTime;
      if (this._dt > 1.0f / this._updateRate) {
        this._fps = this._frameCount / this._dt ;
        this._frameCount = 0;
        this._dt -= 1.0f / this._updateRate;
      }

      this._view.HandleFpsUpdate(this._fps);
    }
  }
}