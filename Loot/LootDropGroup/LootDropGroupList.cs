using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropGroupList : IdList<LootDropGroup> {
    // PRAGMA MARK - Static
    public static LootDropGroupList Instance {
      get {
        return IdListUtil<LootDropGroupList>.Instance;
      }
    }


    // PRAGMA MARK - Public Interface
    public IEnumerable<LootDrop> GetLootDropsForId(int lootDropGroupId) {
      LootDropGroup dropGroup = this.LoadById(lootDropGroupId);
      return (from lootDropId in dropGroup.lootDropIds select Toolbox.GetInstance<LootDropList>().LoadById(lootDropId));
    }
	}
}