using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class Level : MonoBehaviour, ILevel {
		// PRAGMA MARK - ILevel
		public int LevelId {
			get { return _levelId; }
		}
		
		public virtual GameObject SpawnPlayerFromTemplate(int playerIndex, GameObject template) {
			GameObject player = Object.Instantiate(template);
			player.transform.SetParent(transform, worldPositionStays: true);
			return player;
		}
		
		
		// PRAGMA MARK - Internal
		[SerializeField]
		protected int _levelId;
			
		protected virtual void Awake() {
			this.RegisterNotifications();
		}
		
		protected void OnDisable() {
			this.CleanupNotifications();
		}
		
		protected virtual void RegisterNotifications() {
			// do nothing
		}
		
		protected virtual void CleanupNotifications() {
			// do nothing
		}
	}
}