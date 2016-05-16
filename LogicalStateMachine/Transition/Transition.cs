using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class Transition : ITransition {
    // PRAGMA MARK - ITransition Implementation
    public void ConfigureWithContext(TransitionContext context) {
      this._context = context;
    }

    public bool AreConditionsMet() {
      if (!this.CheckConfigured()) {
        return false;
      }

      if (this._waitForManualExit && !this._context.nodeContext.IsManuallyExited) {
        return false;
      }

      foreach (ITransitionCondition condition in this._conditions) {
        if (!condition.IsConditionMet(this._context)) {
          return false;
        }
      }

      return true;
    }

    public void HandleTransitionUsed() {
      if (!this.CheckConfigured()) {
        return;
      }

      foreach (ITransitionCondition condition in this._conditions) {
        condition.HandleTransitionUsed(this._context);
      }
    }

    public void AddTransition(ITransitionCondition condition) {
      this._conditions.Add(condition);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private List<string> _serializedConditions = new List<string>();
    [SerializeField] private bool _waitForManualExit = false;

    private List<ITransitionCondition> _conditions = new List<ITransitionCondition>();

    private TransitionContext _context;

    private bool CheckConfigured() {
      if (this._context == null) {
        Debug.LogError("Transition - not configured, null context!");
        return false;
      }

      return true;
    }
  }
}