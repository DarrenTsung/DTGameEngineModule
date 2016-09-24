using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class DebugLoggerView : MonoBehaviour {
		// PRAGMA MARK - Public Interface
    public void RefreshLogText() {
      this._logOutlet.Text = DebugLogger.GetLogTextToDisplay();
    }


		// PRAGMA MARK - Internal
		[SerializeField] private TextOutlet _logOutlet;

    void OnEnable() {
      this.RefreshLogText();
      DebugLogger.OnLogUpdated += this.RefreshLogText;
    }

    void OnDisable() {
      DebugLogger.OnLogUpdated -= this.RefreshLogText;
    }
	}
}