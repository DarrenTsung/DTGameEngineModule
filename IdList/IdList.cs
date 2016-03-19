using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public static class IdListUtil<TIdList> where TIdList : ScriptableObject, IInitializable {
    // PRAGMA MARK - Static
		private const string RESOURCE_PATH = @"Assets/GameSpecific/Resources";
		private const string IDLIST_FOLDER_NAME = @"IdLists";
		private const string IDLIST_FILE_EXTENSION = @".asset";

    private static TIdList _instance;
		private static object _lock = new object();

    public static TIdList Instance {
      get {
        lock (_lock) {
          if (_instance == null) {
            _instance = IdListUtil<TIdList>.Load();
          }

          return _instance;
        }
      }
    }

#if UNITY_EDITOR
    public static void DirtyInstance() {
      _instance = null;
    }
#endif

    public static TIdList Load() {
      string filename = typeof(TIdList).Name;
			TIdList instance = Resources.Load(IDLIST_FOLDER_NAME + "/" + filename) as TIdList;
#if UNITY_EDITOR
			if (instance == null) {
        string instanceFullPath = RESOURCE_PATH + "/" + IDLIST_FOLDER_NAME + "/" + filename + IDLIST_FILE_EXTENSION;
				if (!AssetDatabase.IsValidFolder(RESOURCE_PATH + "/" + IDLIST_FOLDER_NAME)) {
					AssetDatabase.CreateFolder(RESOURCE_PATH, IDLIST_FOLDER_NAME);
				}
        Debug.Log("Creating new instance of - " + filename);
				instance = ScriptableObject.CreateInstance<TIdList>();
				AssetDatabase.CreateAsset(instance, instanceFullPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
#endif

      instance.Initialize();

			return instance;
    }
  }


  [CustomInspector]
  public abstract class IdList<TEntity> : ScriptableObject, IInitializable, IIdList<TEntity> where TEntity : DTEntity, new() {
    // PRAGMA MARK - IIdList Implementation
    public TEntity LoadById(int id) {
      return this._map.SafeGet(id);
    }

#if UNITY_EDITOR
    public void AddNew() {
      TEntity newObj = new TEntity();
      this._data.Add(newObj);
    }

    public void RemoveAt(int index) {
      this._data.RemoveAt(index);
    }
#endif


    // PRAGMA MARK - IIdList.IEnumerable<TEntity> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<TEntity> GetEnumerator() {
      return this._data.GetEnumerator();
    }


    // PRAGMA MARK - IInitializable Implementation
    public void Initialize() {
      this.CreateMap();
      this.CreateCachedMappings();
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    protected List<TEntity> _data = new List<TEntity>();
    private Dictionary<int, TEntity> _map = new Dictionary<int, TEntity>();

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