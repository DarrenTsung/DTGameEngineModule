using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  [Serializable]
  public partial class IdList<TEntity> : ScriptableObject, IIdList<TEntity> where TEntity : DTEntity {
    // PRAGMA MARK - Static
    public static string ListName() {
      return typeof(TEntity).Name + "List";
    }


    // PRAGMA MARK - Resource Instance
    public static IdList<TEntity> Instance {
      get {
        return IdList<TEntity>.IdListUtil.Instance;
      }
    }

#if UNITY_EDITOR
    public static void DirtyInstance() {
      IdList<TEntity>.IdListUtil.DirtyInstance();
    }
#endif



    // PRAGMA MARK - User Instance
    // NOTE (darren): this event is called in IdList.UserIdListUtil, but is not
    // recognized by the compiler so we need to silence unraised event warnings
#pragma warning disable 67
    public static event Action OnUserInstanceDirtied = delegate {};
#pragma warning restore 67

    public static IdList<TEntity> UserInstance {
      get {
        return IdList<TEntity>.UserIdListUtil.UserInstance;
      }
    }

    public static void DirtyUserInstance() {
      IdList<TEntity>.UserIdListUtil.DirtyUserInstance();
    }



    // PRAGMA MARK - IIdList<TEntity> Implementation
    public TEntity LoadById(int id) {
      if (!this._map.ContainsKey(id)) {
        Debug.LogError("Failed to load entity (" + typeof(TEntity) + ") for id: " + id);
      }

      return this._map.SafeGet(id);
    }

#if UNITY_EDITOR
    public void RemoveAt(int index) {
      this._data.RemoveAt(index);
    }
#endif


    // PRAGMA MARK - IIdList<TEntity>.IEnumerable<TEntity> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<TEntity> GetEnumerator() {
      return this._data.GetEnumerator();
    }


    // PRAGMA MARK - IIdList Implementation
    public IEnumerable<int> Ids() {
      return this._data.Select(e => e.Id());
    }

    DTEntity IIdList.LoadById(int id) {
      return this.LoadById(id);
    }


    // PRAGMA MARK - Internal
    [SerializeField] protected List<TEntity> _data = new List<TEntity>();

    private Dictionary<int, TEntity> _map = new Dictionary<int, TEntity>();

    private void Initialize() {
      foreach (TEntity entity in this._data) {
        DTEntityInitializer.Initialize<TEntity>(entity);
      }

      this.SortDataById();
      this.CreateMap();
      this.CreateCachedMappings();
    }

    // optional
    protected virtual void CreateCachedMappings() {}

    private void CreateMap() {
      this._map.Clear();

      foreach (TEntity entity in this._data) {
        this._map[entity.Id()] = entity;
      }
    }

    private void SortDataById() {
      this._data = this._data.OrderBy(entity => entity.Id()).ToList();
    }
  }
}