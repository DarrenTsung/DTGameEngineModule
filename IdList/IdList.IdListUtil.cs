using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public abstract partial class IdList<TEntity> : ScriptableObject, IIdList<TEntity> where TEntity : DTEntity, new() {
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
  				instance = ScriptableObject.CreateInstance<IdList<TEntity>>();
  				AssetDatabase.CreateAsset(instance, instanceFullPath);
  				AssetDatabase.SaveAssets();
  				AssetDatabase.Refresh();
  			}
  #endif

        instance.Initialize();

  			return instance;
      }
    }
  }
}