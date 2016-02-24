using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(ItemIdAttribute))]
	public class ItemIdDrawer : IdListDrawer<ItemList, Item> {
    protected override string GetDisplayStringForObject(Item obj) {
      return obj.displayName;
    }
	}
}