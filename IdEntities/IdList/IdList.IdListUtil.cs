using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public partial class IdList<TEntity> : ScriptableObject, IEnumerable<TEntity>, IIdList<TEntity> where TEntity : DTEntity, new() {
    private static class IdListUtil {
      // PRAGMA MARK - Static
  		private const string RESOURCE_PATH = @"Assets/GameSpecific/Resources";
  		private const string IDLIST_FOLDER_NAME = @"IdLists";
  		private const string IDLIST_FILE_EXTENSION = @".asset";

      private static IdList<TEntity> _instance;
  		private static object _lock = new object();

      public static IdList<TEntity> Instance {
        get {
          lock (_lock) {
            if (_instance == null) {
              _instance = IdListUtil.Load();
            }

            return _instance;
          }
        }
      }

  #if UNITY_EDITOR
      public static void DirtyInstance() {
        Resources.UnloadAsset(_instance);
        _instance = null;
      }
  #endif

      private static IdList<TEntity> Load() {
        string filename = IdList<TEntity>.ListName();
  			IdList<TEntity> instance = Resources.Load(IDLIST_FOLDER_NAME + "/" + filename) as IdList<TEntity>;
  #if UNITY_EDITOR
  			if (instance == null) {
          string instanceFullPath = RESOURCE_PATH + "/" + IDLIST_FOLDER_NAME + "/" + filename + IDLIST_FILE_EXTENSION;
  				if (!AssetDatabase.IsValidFolder(RESOURCE_PATH + "/" + IDLIST_FOLDER_NAME)) {
  					AssetDatabase.CreateFolder(RESOURCE_PATH, IDLIST_FOLDER_NAME);
  				}
          Debug.Log("Creating new instance of - " + filename);
  				ScriptableObject scriptableObject = ScriptableObject.CreateInstance(filename);
  				AssetDatabase.CreateAsset(scriptableObject, instanceFullPath);
  				AssetDatabase.SaveAssets();
  				AssetDatabase.Refresh();

    			instance = Resources.Load(IDLIST_FOLDER_NAME + "/" + filename) as IdList<TEntity>;
  			}
  #endif

        instance.Initialize();

  			return instance;
      }
    }
  }
}