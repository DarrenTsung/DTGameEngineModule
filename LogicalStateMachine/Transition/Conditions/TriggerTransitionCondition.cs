using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class TriggerTransitionCondition : TransitionCondition {
    // PRAGMA MARK - ITransitionCondition Implementation
    public override bool IsConditionMet(TransitionContext context) {
      return context.graphContext.HasTrigger(this._key);
    }

    public override void HandleTransitionUsed(TransitionContext context) {
      context.graphContext.ResetTrigger(this._key);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private string _key;
  }
}