using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
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
    private List<T> _data = new List<T>();
    private Dictionary<int, T> _map = new Dictionary<int, T>();

    private void Awake() {
      this.RefreshMap();
    }

    private void OnValidate() {
      this.RefreshMap();
    }

    private void RefreshMap() {
      this._map.Clear();

      foreach (T item in this._data) {
        this._map[item.Id] = item;
      }
    }
  }
}