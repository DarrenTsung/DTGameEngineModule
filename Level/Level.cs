using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class Level : MonoBehaviour, ILevel {
		// PRAGMA MARK - Interface
		GameObject ILevel.SpawnPlayerFromTemplate(int playerIndex, GameObject template) {
			GameObject player = Object.Instantiate(template);
			player.transform.SetParent(transform, worldPositionStays: true);
			return player;
		}
	}
}