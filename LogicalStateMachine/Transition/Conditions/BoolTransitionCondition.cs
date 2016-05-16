using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class BoolTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      return this._targetValue == context.graphContext.GetBool(this._key);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
    [SerializeField] private bool _targetValue;
  }
}