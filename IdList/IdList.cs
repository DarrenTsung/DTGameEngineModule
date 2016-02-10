using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [CustomExtensionInspector]
  public class IdList<T> : MonoBehaviour, IEnumerable<T> where T : IIdObject {
    // PRAGMA MARK - Public Interface
    public T LoadById(int id) {
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

    private void Awake() {
      this.HandleDataUpdated();
    }

    private void OnValidate() {
      this.HandleDataUpdated();
    }

    private void HandleDataUpdated() {
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
    private void ReadFromSource() {
      this._data = JsonSerializable.DeserializeFromTextAsset<List<T>>(this._textSource);
    }

    [MakeButton]
    private void WriteToSource() {
      JsonSerializable.SerializeToTextAsset(this._data, this._textSource);
    }
  }
}