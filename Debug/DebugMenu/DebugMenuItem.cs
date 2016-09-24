using DT;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public abstract class DebugMenuItem {
    // PRAGMA MARK - Static
    public const int kDefaultPriority = 100;

    private static DebugMenuItem[] _menuItems;
    public static DebugMenuItem[] MenuItems {
      get {
        if (DebugMenuItem._menuItems == null) {
          DebugMenuItem._menuItems = TypeUtil.GetImplementationTypes(typeof(DebugMenuItem))
                                             .Select(t => (DebugMenuItem)Activator.CreateInstance(t)).ToArray();

          Array.Sort(DebugMenuItem._menuItems, (a, b) => b.Priority.CompareTo(a.Priority));
        }

        return DebugMenuItem._menuItems;
      }
    }


    // PRAGMA MARK - Public Interface
    public abstract string Name { get; }
    public abstract string PrefabName { get; }

    public virtual int Priority {
      get { return kDefaultPriority; }
    }

    public void SetupOn(GameObject uiObject) {
      this.Prefab = ObjectPoolManager.Instantiate(this.PrefabName, parent: uiObject);
    }

    public void Cleanup() {
      if (this.Prefab != null) {
        ObjectPoolManager.Recycle(this.Prefab);
        this.Prefab = null;
      }
    }


    // PRAGMA MARK - Internal
    protected GameObject Prefab {
      get; private set;
    }
  }
}