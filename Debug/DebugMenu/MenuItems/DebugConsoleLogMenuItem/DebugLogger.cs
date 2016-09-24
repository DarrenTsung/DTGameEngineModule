using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public static class DebugLogger {
    // PRAGMA MARK - Static Public Interface
    public static void Initialize() {
      if (DebugLogger._initialized) {
        return;
      }

      DebugLogger._initialized = true;
      Application.logMessageReceived += DebugLogger.HandleLogReceived;
    }

    public static event Action OnLogUpdated = delegate {};

    public static string GetLogTextToDisplay() {
      return string.Join("\n", DebugLogger._bufferedLines.ToArray());
    }


    // PRAGMA MARK - Static Internal
    private const int kBufferLimit = 100;

    private static bool _initialized = false;
    private static Queue<string> _bufferedLines = new Queue<string>();

    private static void HandleLogReceived(string condition, string stackTrace, LogType type) {
      Color logColor = DebugLogger.ColorForLogType(type);
      DebugLogger._bufferedLines.Enqueue(RichTextUtil.WrapWithColorTag(condition, logColor));

      if (DebugLogger._bufferedLines.Count >= kBufferLimit) {
        DebugLogger._bufferedLines.Dequeue();
      }
      DebugLogger.OnLogUpdated.Invoke();
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
	}
}