using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class DebugUIRebuildView : MonoBehaviour {
    // PRAGMA MARK - Internal
    [SerializeField] private Toggle _uiRebuildToggle;

    void OnEnable() {
      this._uiRebuildToggle.isOn = DebugUIRebuildVisualizer.Enabled;
      this._uiRebuildToggle.onValueChanged.AddListener(this.HandleUIRebuildToggleValueChanged);
    }

    void OnDisable() {
      this._uiRebuildToggle.onValueChanged.RemoveListener(this.HandleUIRebuildToggleValueChanged);
    }

    private void HandleUIRebuildToggleValueChanged(bool value) {
      if (value) {
        DebugUIRebuildVisualizer.Enable();
      } else {
        DebugUIRebuildVisualizer.Disable();
      }
    }
	}
}