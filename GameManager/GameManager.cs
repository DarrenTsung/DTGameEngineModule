﻿using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
	public abstract class GameManager : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public void SwitchToSection<T>() {
			// NOTE: should we do anything here?
			this.InternalSwitchToSection<T>();
		}

		public T GetGameSection<T>() where T : class {
			return this.GetGameSectionForType(typeof(T)) as T;
		}

		// PRAGMA MARK - Internal
		protected Dictionary<Type, GameSection> _sections;
		private GameSection _activeSection;

		protected virtual void Awake() {
			this._sections = new Dictionary<Type, GameSection>();
			this.InitializeGameSections();
		}

		protected void Start() {
			this.StartGame();
			this.RegisterNotifications();
		}

		protected void OnDisable() {
			this.CleanupNotifications();
		}

		protected void Update() {
			if (this._activeSection != null) {
				this._activeSection.Update();
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
			this._sections[section.GetType()] = section;
		}

		protected void StartGame() {
			this.InternalSwitchToSectionType(this.StartingSectionKey());
		}

		protected void InternalSwitchToSection<T>() {
			Type sectionType = typeof(T);
			this.InternalSwitchToSectionType(sectionType);
		}

		protected void InternalSwitchToSectionType(Type sectionType) {
			if (this._activeSection != null) {
				this._activeSection.Teardown();
			}

			GameSection newSection = this.GetGameSectionForType(sectionType);
			newSection.Setup();

			this._activeSection = newSection;
		}

		protected GameSection GetGameSectionForType(Type sectionType) {
			if (!this._sections.ContainsKey(sectionType)) {
				foreach (KeyValuePair<Type, GameSection> pair in this._sections) {
					Type classType = pair.Key;
					if (classType.IsSubclassOf(sectionType)) {
						GameSection subclassSection = pair.Value;
						this._sections[sectionType] = subclassSection;
						return subclassSection;
					}
				}
				Debug.LogError("GetGameSectionForType - section type (" + sectionType + ") not found! (also checked subclasses)!");
			}
			return this._sections.SafeGet(sectionType);
		}
	}
}
