using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public static class TimeManager {
    // PRAGMA MARK - Static
    public static DateTime Now {
      get {
        if (!DebugManager.IsDebug) {
          return DateTime.UtcNow;
        }

        return DateTime.UtcNow.AddSeconds(DebugManager.TimeOffset);
      }
    }
  }
}