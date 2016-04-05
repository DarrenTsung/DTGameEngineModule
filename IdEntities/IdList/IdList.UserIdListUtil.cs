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
        UserIdListUtil.CreateDirectoryIfNecessary();

        BinaryFormatter bf = new BinaryFormatter();
        string filepath = UserIdListUtil.Filepath();

        IdList<TEntity> userInstance = UserIdListUtil.UserInstance;
        UserIdListData listData = new UserIdListData(userInstance);

        FileStream file = File.Create(filepath);
        Debug.Log("Saving user instance of - " + IdList<TEntity>.ListName() + "!");
        bf.Serialize(file, listData);
        file.Close();
      }

      private static IdList<TEntity> Load() {
        UserIdListUtil.CreateDirectoryIfNecessary();

        BinaryFormatter bf = new BinaryFormatter();
        string filepath = UserIdListUtil.Filepath();

        if (!File.Exists(filepath)) {
          FileStream file = File.Create(filepath);

          Debug.Log("Creating user instace of - " + IdList<TEntity>.ListName() + " - from resource list by default!");
          IdList<TEntity> defaultedList = IdList<TEntity>.Instance;
          UserIdListData listData = new UserIdListData(defaultedList);

          bf.Serialize(file, listData);
          file.Close();
        }

        FileStream openedFile = File.Open(filepath, FileMode.Open);
        UserIdListData openedListData = (UserIdListData)bf.Deserialize(openedFile);
				IdList<TEntity> userInstance = (IdList<TEntity>)ScriptableObject.CreateInstance(IdList<TEntity>.ListName());
        openedFile.Close();

        userInstance.InitializeWithUserIdListData(openedListData);

  			return userInstance;
      }

      private static string Filepath() {
        string filename = IdList<TEntity>.ListName();
        return UserIdListUtil.DirectoryPath() + "/" + filename + USER_ID_LIST_FILE_EXTENSION;
      }

      private static string DirectoryPath() {
        return Application.persistentDataPath + "/" + USER_ID_LIST_FOLDER_NAME;
      }

      private static void CreateDirectoryIfNecessary() {
        string directoryPath = UserIdListUtil.DirectoryPath();
        if (Directory.Exists(directoryPath)) {
          return;
        }

        Directory.CreateDirectory(directoryPath);
      }
    }
  }
}