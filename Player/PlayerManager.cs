using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class PlayerManager : Singleton<PlayerManager> {
		protected PlayerManager() {}
		
		protected void SetPlayer(int playerIndex, GameObject player) {
			_players[playerIndex] = player;
			DTNotifications.PlayerChanged.Invoke(playerIndex, player);
		}
		
		// PRAGMA MARK - INTERNAL
		[Header("---- PlayerManager Properties ----")]
		[SerializeField]
		protected List<Player> _startingPlayers;
		protected Dictionary<int, GameObject> _players;
		
		protected void Awake() {
			_players = new Dictionary<int, GameObject>();
		}
		
		protected void Start() {
			if (_startingPlayers.Count == 0) {
				Debug.LogError("PlayerManager - no starting players!");
			}
			
			int playerIndex = 0;
			foreach (Player player in _startingPlayers) {
				player.PlayerIndex = playerIndex;
				this.SetPlayer(playerIndex, player.gameObject);
				playerIndex++;
			}
		}
	}
}