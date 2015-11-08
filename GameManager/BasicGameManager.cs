using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT.GameEngine {
	public enum GameSectionKey {
		REGISTRATION 
	}
	
	public class BasicGameManager : GameManager {
		// PRAGMA MARK - Internal
		[Header("BASIC - Player Registration Properties")]
		[SerializeField]
		protected InputControlType _registerControlType = InputControlType.Action1;
		[SerializeField]
		protected InputControlType _finishRegistrationControlType = InputControlType.Command;
    
		[Header("BASIC - Level Simulation Properties")]
		[SerializeField]
		protected GameObject _levelPrefab;
		[SerializeField]
		protected GameObject _playerTemplatePrefab;
		
		protected override void RegisterNotifications() {
      base.RegisterNotifications();
			DTGameEngineNotifications.PlayerRegistrationFinished.AddListener(this.HandlePlayerRegistrationFinished);
		}
		
		protected override void CleanupNotifications() {
      base.CleanupNotifications();
			DTGameEngineNotifications.PlayerRegistrationFinished.RemoveListener(this.HandlePlayerRegistrationFinished);
		}
		
		protected virtual void HandlePlayerRegistrationFinished() {
      this.SwitchToSection<LevelSimulationGameSection>();
    }
		
		protected override void InitializeInheritedGameSections() {
			this.AddGameSection(new PlayerRegistrationGameSection(_registerControlType, _finishRegistrationControlType));
      this.AddGameSection(new LevelSimulationGameSection(_levelPrefab, _playerTemplatePrefab));
    }
      
		protected override Type StartingSectionKey() {
      return typeof(PlayerRegistrationGameSection);
    }
	}
}

#endif