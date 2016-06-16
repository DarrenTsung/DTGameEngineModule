using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class DebugLoggerView : MonoBehaviour {
    // PRAGMA MARK - Static Public Interface
    public static void Initialize() {
      if (DebugLoggerView._initialized) {
        return;
      }

      DebugLoggerView._initialized = true;
      Application.logMessageReceived += DebugLoggerView.HandleLogReceieved;
    }

    public static event Action OnLogUpdated = delegate {};

    public static string GetLogTextToDisplay() {
      return string.Join("\n", DebugLoggerView._bufferedLines.ToArray());
    }


    // PRAGMA MARK - Static Internal
    private const int kBufferLimit = 100;

    private static bool _initialized = false;

    static DebugLoggerView() {
      DebugLoggerView.Initialize();
    }

    private static Queue<string> _bufferedLines = new Queue<string>();

    private static void HandleLogReceieved(string condition, string stackTrace, LogType type) {
      Color logColor = DebugLoggerView.ColorForLogType(type);
      DebugLoggerView._bufferedLines.Enqueue(RichTextUtil.WrapWithColorTag(condition, logColor));

      if (DebugLoggerView._bufferedLines.Count >= kBufferLimit) {
        DebugLoggerView._bufferedLines.Dequeue();
      }
      DebugLoggerView.OnLogUpdated.Invoke();
    }

    private static Color ColorForLogType(LogType type) {
      switch (type) {
        case LogType.Warning:
          return Color.yellow;
        case LogType.Error:
          return Color.red;
        default:
          return Color.white;
      }
    }


		// PRAGMA MARK - Public Interface
    public void RefreshLogText() {
      this._logOutlet.Text = DebugLoggerView.GetLogTextToDisplay();
    }


		// PRAGMA MARK - Internal
		[SerializeField] private TextOutlet _logOutlet;

    void OnEnable() {
      this.RefreshLogText();
      DebugLoggerView.OnLogUpdated += this.RefreshLogText;
    }

    void OnDisable() {
      DebugLoggerView.OnLogUpdated -= this.RefreshLogText;
    }
	}
}