using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class LevelSimulationGameSection : GameSection {
		// PRAGMA MARK - Interface
		public LevelSimulationGameSection(GameObject levelPrefab,
																			GameObject playerTemplatePrefab) {
		  _levelPrefab = levelPrefab;
			_playerTemplatePrefab = playerTemplatePrefab;
		}
		
		// not passing in prefabs is not allowed
		protected LevelSimulationGameSection() {}
		
		
		// PRAGMA MARK - Internal
		protected GameObject _levelPrefab;
		protected GameObject _playerTemplatePrefab;
		
		protected GameObject _level;
		
		protected override void InternalSetup() {
			if (_levelPrefab == null || _playerTemplatePrefab == null) {
				Debug.LogError("LevelSimulationGameSection::InternalSetup - no level or player template prefab!");
				return;
			}
			
			GameObject _level = Object.Instantiate(_levelPrefab) as GameObject;
			_level.transform.SetParent(_context.transform, worldPositionStays: true);
			ILevel levelComponent = _level.GetComponent<ILevel>();
			
			if (levelComponent == null) {
				Debug.LogError("LevelSimulationGameSection::InternalSetup - no ILevel component on level prefab!");
			}
			
			foreach (int playerIndex in Toolbox.GetInstance<IPlayerInputManager>().UsedPlayerIndexes()) {
				GameObject player = levelComponent.SpawnPlayerFromTemplate(playerIndex, _playerTemplatePrefab);
				Toolbox.GetInstance<PlayerManager>().SetPlayer(playerIndex, player);
			}
		}
		
		protected override void InternalTeardown() {
			Object.Destroy(_level);
		}
	}
}