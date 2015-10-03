using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class PlayerManager : Singleton<PlayerManager> {
		protected PlayerManager() {}
		
		public GameObject Player {
			get { return _player; }
			set { 
				_player = value; 
				this.OnPlayerChange.Invoke(_player);
			}
		}
		
		public UnityEvents.O OnPlayerChange;
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected GameObject _startPlayer;
		protected GameObject _player;
		
		protected void Start() {
			if (_startPlayer) {
				this.Player = _startPlayer;
			} else {
				Debug.LogError("PlayerManager - no player linked to manager!");
			}
		}
	}
}