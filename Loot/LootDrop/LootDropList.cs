using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropList : IdList<LootDrop> {
    // PRAGMA MARK - Public Interface
    public LootDrop[] GetLootDropsForGroupId(int lootDropGroupId) {
      return this._groupIdMap.SafeGet(lootDropGroupId) ?? new LootDrop[0];
    }


    // PRAGMA MARK - Internal
    private Dictionary<int, LootDrop[]> _groupIdMap = new Dictionary<int, LootDrop[]>();

    protected override void RefreshCachedMappings() {
      this._groupIdMap.Clear();

      Dictionary<int, List<LootDrop>> tempGroupMap = new Dictionary<int, List<LootDrop>>();
      foreach (LootDrop lootDrop in this._data) {
        List<LootDrop> lootDrops = tempGroupMap.GetAndCreateIfNotFound(lootDrop.GroupId);
        lootDrops.Add(lootDrop);
      }

      foreach (KeyValuePair<int, List<LootDrop>> pair in tempGroupMap) {
        this._groupIdMap[pair.Key] = pair.Value.ToArray();
      }
    }
	}
}