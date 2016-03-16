using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public static class IdListUtil<TIdList> where TIdList : ScriptableObject {
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
			return instance;
    }
  }


  [CustomInspector]
  public abstract class IdList<TObject> : ScriptableObject, IIdList<TObject> where TObject : IIdObject {
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

    private void Refresh() {
      this.RefreshMap();
      this.RefreshCachedMappings();
    }

    // optional
    protected virtual void RefreshCachedMappings() {}

    private void RefreshMap() {
      this._map.Clear();

      foreach (TObject item in this._data) {
        if (item == null) {
          continue;
        }

        this._map[item.Id] = item;
      }
    }
  }
}