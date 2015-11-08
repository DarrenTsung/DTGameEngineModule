using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
	public abstract class GameManager : MonoBehaviour {
		// PRAGMA MARK - Interface
		public void SwitchToSection<T>() {
			// NOTE: should we do anything here?
			this.InternalSwitchToSection<T>();
		}
		
		public T GetGameSection<T>() where T : class {
			return _sections.SafeGet(typeof(T)) as T;
		}
		
		// PRAGMA MARK - Internal
		protected Dictionary<Type, GameSection> _sections;
		private GameSection _activeSection;
		
		protected void Awake() {
			_sections = new Dictionary<Type, GameSection>();
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
			// do nothing
		}
		
		protected virtual void CleanupNotifications() {
			// do nothing
		}
		
		protected void InitializeGameSections() {
			this.InitializeInheritedGameSections();
		}
		
		protected abstract void InitializeInheritedGameSections();
		
		protected abstract Type StartingSectionKey();
		
		protected void AddGameSection(GameSection section) {
			section.SetContext(this);
			_sections[section.GetType()] = section;
		}
		
		protected void StartGame() {
			this.InternalSwitchToSectionType(this.StartingSectionKey());		
		}
		
		protected void InternalSwitchToSection<T>() {
			Type sectionType = typeof(T);
			this.InternalSwitchToSectionType(sectionType);
		}
		
		protected void InternalSwitchToSectionType(Type sectionType) {
			if (_activeSection != null) {
				_activeSection.Teardown();
			}
			
			if (!_sections.ContainsKey(sectionType)) {
				Debug.LogError("SwitchToSection - invalid section type (" + sectionType + ") to switch to!");
				return;
			}
			
			GameSection newSection = _sections[sectionType];
			newSection.Setup();
			
			_activeSection = newSection;
		}
	}
}
