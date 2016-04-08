using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
  public partial class IdList<TEntity> : ScriptableObject, IEnumerable<TEntity>, IIdList<TEntity> where TEntity : DTEntity {
    private void InitializeWithUserIdListData(UserIdListData listData) {
      this._data = listData.data;
      this.Initialize();
    }

    [Serializable]
    private class UserIdListData {
      [SerializeField]
      public List<TEntity> data;

      public UserIdListData(IdList<TEntity> list) {
        this.data = list._data;
      }
    }

    private static class UserIdListUtil {
      // PRAGMA MARK - Static
  		private const string USER_ID_LIST_FOLDER_NAME = @"UserIdLists";
  		private const string USER_ID_LIST_FILE_EXTENSION = @".dat";

      private static IdList<TEntity> _userInstance;
      private static bool _instanceDirtied;
  		private static object _lock = new object();

      public static IdList<TEntity> UserInstance {
        get {
          lock (_lock) {
            if (_userInstance == null) {
              _userInstance = UserIdListUtil.Load();
            }

            return _userInstance;
          }
        }
      }

      public static void DirtyUserInstance() {
        _instanceDirtied = true;
        // TODO (darren): move this check to be checked every 30 seconds or something
        UserIdListUtil.SaveUserInstanceIfDirty();
      }

      private static void SaveUserInstanceIfDirty() {
        if (_instanceDirtied) {
          UserIdListUtil.Save();
          _instanceDirtied = false;
        }
      }

      private static void Save() {
        UserIdListData listData = new UserIdListData(UserIdListUtil._userInstance);
        PersistentDataUtil.Save(UserIdListUtil.DirectoryPath(), UserIdListUtil.Filename(), listData);
      }

      private static IdList<TEntity> Load() {
        UserIdListData listData = PersistentDataUtil.Load<UserIdListData>(UserIdListUtil.DirectoryPath(), UserIdListUtil.Filename(), UserIdListUtil.CreateDefaultInstance);

				IdList<TEntity> userInstance = (IdList<TEntity>)ScriptableObject.CreateInstance(IdList<TEntity>.ListName());
        userInstance.InitializeWithUserIdListData(listData);

        return userInstance;
      }

      private static string Filename() {
        return IdList<TEntity>.ListName() + USER_ID_LIST_FILE_EXTENSION;
      }

      private static string DirectoryPath() {
        return USER_ID_LIST_FOLDER_NAME;
      }

      private static UserIdListData CreateDefaultInstance() {
        IdList<TEntity> defaultedList = IdList<TEntity>.Instance;
        return new UserIdListData(defaultedList);
      }
    }
  }
}