using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropGroupManager {
    // PRAGMA MARK - Public Interface
    public static List<LootDrop> GetLootDropsForId(int lootDropGroupId) {
      IIdList<LootDrop> lootDropList = ListFactory<LootDrop>.Instance.GetList();
      LootDropGroup dropGroup = IdList<LootDropGroup>.Instance.LoadById(lootDropGroupId);
      return (from lootDropId in dropGroup.lootDropIds select lootDropList.LoadById(lootDropId)).ToList();
    }
	}
}
