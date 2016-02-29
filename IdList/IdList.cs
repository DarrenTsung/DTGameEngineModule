using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [CustomInspector]
  public class IdList<T> : MonoBehaviour, IEnumerable<T> where T : IIdObject {
    // PRAGMA MARK - Public Interface
    public T LoadById(int id) {
      if (!this._initialized) {
        this.Refresh();
        this._initialized = true;
      }

      return this._map.SafeGet(id);
    }


    // PRAGMA MARK - IEnumerable<T> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator() {
      return this._data.GetEnumerator();
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    protected List<T> _data = new List<T>();
    [SerializeField]
    private TextAsset _textSource;
    private Dictionary<int, T> _map = new Dictionary<int, T>();

    private bool _initialized = false;

    private void OnValidate() {
      this._initialized = false;
    }

    private void Refresh() {
      this.RefreshMap();
      this.RefreshCachedMappings();
    }

    // optional
    protected virtual void RefreshCachedMappings() {}

    private void RefreshMap() {
      this._map.Clear();

      foreach (T item in this._data) {
        this._map[item.Id] = item;
      }
    }

    [MakeButton]
    protected void ReadFromSource() {
      JsonSerializable.OverwriteDeserializeFromTextAsset(this._textSource, this);
    }

    [MakeButton]
    protected void WriteToSource() {
      JsonSerializable.SerializeToTextAsset(this, this._textSource, prettyPrint: true);
    }
  }
}