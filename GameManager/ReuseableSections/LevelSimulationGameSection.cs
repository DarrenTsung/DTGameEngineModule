using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class LevelSimulationGameSection : GameSection {
		// PRAGMA MARK - Constructors
		public LevelSimulationGameSection(GameObject levelPrefab,
																			GameObject playerTemplatePrefab) {
		  _levelPrefab = levelPrefab;
			_playerTemplatePrefab = playerTemplatePrefab;
		}
		
		// not passing in no prefabs is not allowed
		protected LevelSimulationGameSection() {}
		
		
		// PRAGMA MARK - Public Interface
		public GameObject CurrentLevel {
			get { return _level; }
		}
		
		public ILevel CurrentLevelInterface {
			get { return _levelInterfaceComponent; }
		}
		
		
		// PRAGMA MARK - Internal
		protected GameObject _levelPrefab;
		protected GameObject _playerTemplatePrefab;
		
		protected GameObject _level;
		protected ILevel _levelInterfaceComponent;
		
		protected override void InternalSetup() {
			if (_levelPrefab == null || _playerTemplatePrefab == null) {
				Debug.LogError("LevelSimulationGameSection::InternalSetup - no level or player template prefab!");
				return;
			}
			
			this.SetupWithLevelObject(Object.Instantiate(_levelPrefab) as GameObject);
			
			DTGameEngineNotifications.OnLevelSimulationSectionSetup.Invoke();
		}
		
		protected virtual void SetupWithLevelObject(GameObject levelObject) {
			_level = levelObject;
			_level.transform.SetParent(_context.transform, worldPositionStays: true);
			_levelInterfaceComponent = _level.GetRequiredComponent<ILevel>();
			
			foreach (int playerIndex in Toolbox.GetInstance<IPlayerInputManager>().UsedPlayerIndexes()) {
				GameObject player = _levelInterfaceComponent.SpawnPlayerFromTemplate(playerIndex, _playerTemplatePrefab);
				Toolbox.GetInstance<PlayerManager>().SetPlayer(playerIndex, player);
			}
		}
		
		protected override void InternalTeardown() {
			Object.Destroy(_level);
			
			DTGameEngineNotifications.OnLevelSimulationSectionTeardown.Invoke();
		}
	}
}