using DT;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class DTEntity {
    // PRAGMA MARK - Public Interface
    public DTEntity() {
#if UNITY_EDITOR
      // only do this expensive initialization if we're being created in the unity editor
      DTEntityInitializer.Initialize(this.GetType(), this);
#endif
    }

    public bool Initialized {
      get { return this._initialized; }
    }

    public void FinishInitializing() {
      if (this.Initialized) {
        Debug.LogWarning("FinishInitializing - called when already Initialized!");
        return;
      }

      this._initialized = true;
      this.HandleFinishedInitializing();
    }

    public TComponent GetComponent<TComponent>() where TComponent : class, IDTComponent {
      return this.GetComponent(typeof(TComponent)) as TComponent;
    }

    public object GetComponent(Type type) {
      if (!this.Initialized) {
        Debug.LogWarning("GetComponent - called on an uninitialized DTEntity!");
      }
      return this._componentMap.SafeGet(type);
    }

    public bool HasComponent<TComponent>() where TComponent : class, IDTComponent {
      return this.HasComponent(typeof(TComponent));
    }

    public bool HasComponent(Type type) {
      if (!this.Initialized) {
        Debug.LogWarning("HasComponent - called on an uninitialized DTEntity!");
      }
      return this._componentMap.ContainsKey(type);
    }

    public void AddComponent<TComponent>(TComponent component) {
      this.AddComponent(typeof(TComponent), component);
    }

    public void AddComponent(Type type, object component) {
      this._componentMap[type] = component;
    }


    // PRAGMA MARK - Internal
    private Dictionary<Type, object> _componentMap = new Dictionary<Type, object>();
    [NonSerialized]
    private bool _initialized = false;

    private void HandleFinishedInitializing() {

    }
  }
}