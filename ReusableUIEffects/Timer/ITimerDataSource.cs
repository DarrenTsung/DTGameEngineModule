using DT;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public interface ITimerDataSource {
    float PercentageComplete();
    string TimerText();
  }
}