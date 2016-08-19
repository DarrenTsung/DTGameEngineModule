using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.GameEngine {
  public static class TimeManagerDateTimeExtensions {
    public static int IntSecondsPassedFromTimeManagerNow(this DateTime time) {
      return time.IntSecondsPassedFrom(TimeManager.Now);
    }

    public static float SecondsPassedFromTimeManagerNow(this DateTime time) {
      return time.SecondsPassedFrom(TimeManager.Now);
    }
  }
}