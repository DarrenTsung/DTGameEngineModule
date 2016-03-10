using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [CustomInspector]
  public abstract class IdList<TObject, TSingleton> : IIdList<TObject> where TObject : IIdObject
                                                                       where TSingleton : IIdList<TObject>, new() {
    // PRAGMA MARK - Static
    private static TSingleton _instance;
		private static object _lock = new object();

    public static TSingleton Instance {
      get {
        lock (_lock) {
          if (_instance == null) {
            _instance = new TSingleton();
          }

          return _instance;
        }
      }
    }


    // Constructor is public if you are in the editor because we want to enforce the constraint
    // that production code (non-editor) uses the Instance for performance instead of creating
    // new instances
#if UNITY_EDITOR
    public IdList() {
      this.Initialize();
    }
#else
    private IdList() {
      this.Initialize();
    }
#endif


    // PRAGMA MARK - IIdList Implementation
    public TObject LoadById(int id) {
      return this._map.SafeGet(id);
    }

#if UNITY_EDITOR
    public void Add(TObject newObj) {
      this._data.Add(newObj);
    }

    public void RemoveAt(int index) {
      this._data.RemoveAt(index);
    }

    public void SaveChanges() {
      JsonSerialization.SerializeToTextAssetFilename(this, this.Filename);
    }
#endif


    // PRAGMA MARK - IIdList.IEnumerable<TObject> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<TObject> GetEnumerator() {
      return this._data.GetEnumerator();
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    protected List<TObject> _data = new List<TObject>();
    private Dictionary<int, TObject> _map = new Dictionary<int, TObject>();

    private void Initialize() {
      TextAsset asset = TextAssetUtil.GetOrCreateTextAsset(this.Filename);
      if (asset == null) {
        return;
      }

      JsonSerialization.OverwriteDeserializeFromTextAsset(asset, this);
      this.Refresh();
    }

    private string Filename {
      get {
        return this.GetType().Name;
      }
    }

    private void Refresh() {
      this.RefreshMap();
      this.RefreshCachedMappings();
    }

    // optional
    protected virtual void RefreshCachedMappings() {}

    private void RefreshMap() {
      this._map.Clear();

      foreach (TObject item in this._data) {
        this._map[item.Id] = item;
      }
    }
  }
}