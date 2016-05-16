using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class IntTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      if (!context.graphContext.HasIntParameterKey(this._key)) {
        return false;
      }

      return this._targetValue == context.graphContext.GetInt(this._key);
    }

    public IntTransitionCondition(string key, int targetValue) {
      this._key = key;
      this._targetValue = targetValue;
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
    [SerializeField] private int _targetValue;
  }
}