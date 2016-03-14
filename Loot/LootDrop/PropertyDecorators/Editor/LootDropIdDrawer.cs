using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(LootDropIdAttribute))]
	public class LootDropIdDrawer : IdListDrawer<LootDropList, LootDrop> {
	}
}