using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(LootDropGroupIdAttribute))]
	public class LootDropGroupIdDrawer : IdListDrawer<LootDropGroupList, LootDropGroup> {
	}
}