using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropGroupList : IdList<LootDropGroup> {
    // PRAGMA MARK - Public Interface
    public static List<LootDrop> GetLootDropsForId(int lootDropGroupId) {
      LootDropGroup dropGroup = IdList<LootDropGroup>.Instance.LoadById(lootDropGroupId);
      return (from lootDropId in dropGroup.lootDropIds select LootDropList.Instance.LoadById(lootDropId)).ToList();
    }
	}
}