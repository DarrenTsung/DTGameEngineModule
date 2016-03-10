using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropGroupList : IdList<LootDropGroup, LootDropGroupList> {
    // PRAGMA MARK - Public Interface
    public IEnumerable<LootDrop> GetLootDropsForId(int lootDropGroupId) {
      LootDropGroup dropGroup = this.LoadById(lootDropGroupId);
      return (from lootDropId in dropGroup.lootDropIds select Toolbox.GetInstance<LootDropList>().LoadById(lootDropId));
    }
	}
}