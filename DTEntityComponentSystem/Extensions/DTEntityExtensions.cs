using DT;
using System;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class DTEntityExtensions {
    public static void PopulateSpriteRenderer(this DTEntity entity, SpriteRenderer spriteRenderer) {
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent == null) {
        Debug.LogError("PopulateSpriteRenderer - failed to get display component!");
      }

      spriteRenderer.sprite = displayComponent.displaySprite;
    }
	}
}