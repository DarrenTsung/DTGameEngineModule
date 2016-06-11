using DT;
using System;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
  public static class DTEntityExtensions {
    public static void PopulateSpriteRenderer(this DTEntity entity, SpriteRenderer spriteRenderer) {
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent == null) {
        Debug.LogError("PopulateSpriteRenderer - failed to get display component!");
        return;
      }

      spriteRenderer.sprite = displayComponent.displaySprite;
    }

    public static void PopulateImage(this DTEntity entity, Image image) {
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent == null) {
        Debug.LogError("PopulateImage - failed to get display component!");
        return;
      }

      image.sprite = displayComponent.displaySprite;
    }

    public static void PopulateSpriteOutlet(this DTEntity entity, SpriteOutlet spriteOutlet) {
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent == null) {
        Debug.LogError("PopulateSpriteOutlet - failed to get display component!");
        return;
      }

      spriteOutlet.Sprite = displayComponent.displaySprite;
    }

    public static string DisplayName(this DTEntity entity) {
      return entity.GetComponent<DisplayComponent>().displayName;
    }

    public static Sprite DisplaySprite(this DTEntity entity) {
      return entity.GetComponent<DisplayComponent>().displaySprite;
    }

    public static void PopulateNameOutlet(this DTEntity entity, TextOutlet textOutlet) {
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent == null) {
        Debug.LogError("PopulateNameOutlet - failed to get display component!");
        return;
      }

      textOutlet.Text = displayComponent.displayName;
    }
	}
}