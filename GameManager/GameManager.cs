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
	
	public abstract class GameManager : MonoBehaviour {
		// PRAGMA MARK - Interface
		public void SwitchToSection(Enum sectionKey) {
			// NOTE: should we do anything here?
			this.InternalSwitchToSection(sectionKey);
		}
		
		// PRAGMA MARK - Internal
		[Header("Player Registration Properties")]
		[SerializeField]
		protected bool _playerRegistrationEnabled;
		[SerializeField]
		protected InputControlType _registerControlType;
		[SerializeField]
		protected InputControlType _finishRegistrationControlType;
	
		protected Dictionary<Enum, GameSection> _sections;
		private GameSection _activeSection;
		
		protected void Awake() {
			_sections = new Dictionary<Enum, GameSection>();
		}
		
		protected void Start() {
			this.InitializeGameSections();
			this.StartGame();
			this.RegisterNotifications();
		}
		
		protected void OnDisable() {
			this.CleanupNotifications();
		}
		
		protected void Update() {
			if (_activeSection != null) {
				_activeSection.Update();
			}
		}
		
		protected virtual void RegisterNotifications() {
			DTGameEngineNotifications.PlayerRegistrationFinished.AddListener(this.HandlePlayerRegistrationFinished);
		}
		
		protected virtual void CleanupNotifications() {
			DTGameEngineNotifications.PlayerRegistrationFinished.RemoveListener(this.HandlePlayerRegistrationFinished);
		}
		
		protected virtual void HandlePlayerRegistrationFinished() {}
		
		protected void InitializeGameSections() {
			this.MapGameSectionWithKey(GameSectionKey.REGISTRATION, new PlayerRegistrationGameSection(_registerControlType, _finishRegistrationControlType));
			this.InitializeInheritedGameSections();
		}
		
		protected abstract void InitializeInheritedGameSections();
		
		protected abstract Enum StartingSectionKey();
		
		protected void MapGameSectionWithKey(Enum key, GameSection section) {
			_sections[key] = section;
		}
		
		protected void StartGame() {
			this.InternalSwitchToSection(this.StartingSectionKey());
		}
		
		protected void InternalSwitchToSection(Enum sectionKey) {
			if (_activeSection != null) {
				_activeSection.Teardown();
			}
			
			if (!_sections.ContainsKey(sectionKey)) {
				Debug.LogError("SwitchToSection - invalid section key to switch to!");
				return;
			}
			
			GameSection newSection = _sections[sectionKey];
			newSection.Setup();
			
			_activeSection = newSection;
		}
	}
}

#endif