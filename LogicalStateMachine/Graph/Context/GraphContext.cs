using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class GraphContext : IGraphContext {
    // PRAGMA MARK - Public Interface
    public event Action OnContextUpdated = delegate {};

    public int GetInt(string key) {
      if (!this._intValues.ContainsKey(key)) {
        Debug.LogError("GraphContext - GetInt called with invalid key: " + key);
        return 0;
      }

      return this._intValues[key];
    }

    public void SetInt(string key, int val) {
      this._intValues[key] = val;
      this.OnContextUpdated.Invoke();
    }

    public bool GetBool(string key) {
      if (!this._boolValues.ContainsKey(key)) {
        Debug.LogError("GraphContext - GetBool called with invalid key: " + key);
        return false;
      }

      return this._boolValues[key];
    }

    public void SetBool(string key, bool val) {
      this._boolValues[key] = val;
      this.OnContextUpdated.Invoke();
    }

    public bool HasTrigger(string key) {
      if (!this._triggerValues.ContainsKey(key)) {
        Debug.LogError("GraphContext - HasTrigger called with invalid key: " + key);
        return false;
      }

      return this._triggerValues[key];
    }

    public void SetTrigger(string key) {
      this._triggerValues[key] = true;
      this.OnContextUpdated.Invoke();
    }

    public void ResetTrigger(string key) {
      this._triggerValues[key] = false;
      this.OnContextUpdated.Invoke();
    }

    public void PopulateStartingContextParameters(IList<GraphContextParameter> contextParameters) {
      foreach (GraphContextParameter contextParameter in contextParameters) {
        switch (contextParameter.type) {
          case GraphContextParameterType.Int:
            this._intValues[contextParameter.key] = 0;
            break;
          case GraphContextParameterType.Bool:
            this._boolValues[contextParameter.key] = false;
            break;
          case GraphContextParameterType.Trigger:
            this._triggerValues[contextParameter.key] = false;
            break;
        }
      }
    }


    // PRAGMA MARK - Internal
    private Dictionary<string, int> _intValues = new Dictionary<string, int>();
    private Dictionary<string, bool> _boolValues = new Dictionary<string, bool>();
    private Dictionary<string, bool> _triggerValues = new Dictionary<string, bool>();
  }
}