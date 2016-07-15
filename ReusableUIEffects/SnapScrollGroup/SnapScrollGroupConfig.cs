using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class SnapScrollGroupConfig {
    [Header("Screen-space Properties")]
    public float spacing = 1.0f;

    [Header("Outlets")]
    public SnapScrollPointerHandler pointerHandler = null;

    [Header("Properties")]
    // ex. leftShown = 1 ===> the view directly left of
    // the current index will be shown
    public int leftShown = 2;
    public int rightShown = 2;

    public float snapDuration = 0.6f;

    public Axis axis = Axis.HORIZONTAL;

    public bool IsInvalid() {
      // NOTE (darren): if things must be filled out, check for them here
      if (this.pointerHandler == null) {
        return true;
      }

      return false;
    }
  }
}