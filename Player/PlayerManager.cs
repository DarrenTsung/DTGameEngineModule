﻿using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class PlayerManager : MonoBehaviour {
		protected PlayerManager() {}
		
		// PRAGMA MARK - Interface
		public void SetPlayer(int playerIndex, GameObject player) {
			GameObject previousPlayer = _players.SafeGet(playerIndex, null);
			if (previousPlayer != null) {
				Destroy(previousPlayer);
			}
			_players[playerIndex] = player;
			
			Player playerComponent = player.GetComponent<Player>();
			playerComponent.PlayerIndex = playerIndex;
			
			DTNotifications.PlayerChanged.Invoke(playerIndex, player);
		}
		
		// PRAGMA MARK - Internal
		[Header("---- PlayerManager Properties ----")]
		protected Dictionary<int, GameObject> _players;
		
		protected virtual void Awake() {
			_players = new Dictionary<int, GameObject>();
		}
	}
}