using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public partial class UserIdInventory<TEntity> where TEntity : DTEntity {
    // PRAGMA MARK - Static
    private static class InstanceUtil {
  		private const string USER_ID_INVENTORY_FOLDER_NAME = @"UserIdInventories";
  		private const string USER_ID_INVENTORY_FILE_EXTENSION = @".dat";

      private static UserIdInventory<TEntity> _instance;
  		private static object _lock = new object();
      private static bool _instanceDirtied;

      public static UserIdInventory<TEntity> Instance {
        get {
          lock (_lock) {
            if (_instance == null) {
              _instance = InstanceUtil.Load();
            }

            return _instance;
          }
        }
      }

      public static void DirtyInstance() {
        _instanceDirtied = true;
        // TODO (darren): move this check to be checked every 30 seconds or something
        InstanceUtil.SaveInstanceIfDirty();
      }

      private static void SaveInstanceIfDirty() {
        if (_instanceDirtied) {
          InstanceUtil.Save();
          _instanceDirtied = false;
        }
      }

      private static void Save() {
        PersistentDataUtil.Save(InstanceUtil.DirectoryPath(), InstanceUtil.Filename(), InstanceUtil._instance);
      }

      private static UserIdInventory<TEntity> Load() {
        return PersistentDataUtil.Load<UserIdInventory<TEntity>>(InstanceUtil.DirectoryPath(), InstanceUtil.Filename(), InstanceUtil.CreateDefaultInstance);
      }

      private static string Filename() {
        return "UserIdInventory" + typeof(TEntity).Name + USER_ID_INVENTORY_FILE_EXTENSION;
      }

      private static string DirectoryPath() {
        return USER_ID_INVENTORY_FOLDER_NAME;
      }

      private static UserIdInventory<TEntity> CreateDefaultInstance() {
        return new UserIdInventory<TEntity>();
      }
    }
  }
}