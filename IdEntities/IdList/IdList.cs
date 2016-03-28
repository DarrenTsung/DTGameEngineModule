using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public partial class IdList<TEntity> : ScriptableObject, IIdList<TEntity> where TEntity : DTEntity, new() {
    // PRAGMA MARK - Static
    public static string ListName() {
      return typeof(TEntity).Name + "List";
    }

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


    // PRAGMA MARK - IIdList<TEntity> Implementation
    public TEntity LoadById(int id) {
      return this._map.SafeGet(id);
    }

#if UNITY_EDITOR
    public void AddNew() {
      TEntity newEntity = new TEntity();
      DTEntityInitializer.Initialize<TEntity>(newEntity);
      this._data.Add(newEntity);
    }

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


    // PRAGMA MARK - Internal
    [SerializeField]
    protected List<TEntity> _data = new List<TEntity>();
    private Dictionary<int, TEntity> _map = new Dictionary<int, TEntity>();

    private void Initialize() {
      this.CreateMap();
      this.CreateCachedMappings();
    }

    // optional
    protected virtual void CreateCachedMappings() {}

    private void CreateMap() {
      this._map.Clear();

      foreach (TEntity entity in this._data) {
        if (entity == null) {
          continue;
        }

        DTEntityInitializer.Initialize<TEntity>(entity);

        IdComponent idComponent = entity.GetComponent<IdComponent>();
        if (idComponent == null) {
          Debug.LogError("IdList - entity does not have IdComponent!");
          continue;
        }

        this._map[idComponent.id] = entity;
      }
    }
  }
}