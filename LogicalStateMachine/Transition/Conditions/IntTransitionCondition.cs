using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class IntTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      return this._targetValue == context.graphContext.GetInt(this._key);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
    [SerializeField] private int _targetValue;
  }
}