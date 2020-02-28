using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Merge UI prompt sorting by distance into InteractZone logic.

public class UIPromptManager : Singleton<UIPromptManager> {

  Dictionary<Transform, UIPrompt> prompts = new Dictionary<Transform, UIPrompt>();
  UIPrompt currentPrompt;

  Transform player;
  Vector3 lastPosition;

  void Update() {
    if (player != null && player.position != lastPosition) {
      lastPosition = player.position;
      RenderPrompt();
    }
  }

  public void SetPlayer(Transform player) {
    this.player = player;
  }

  public void AddPrompt(UIPrompt prompt, Transform location) {
    if (!prompts.TryGetValue(location, out UIPrompt value)) {
      prompts.Add(location, prompt);
    }
  }

  public void RemovePrompt(UIPrompt prompt, Transform location) {
    if (prompts.TryGetValue(location, out UIPrompt value)) {
      prompts.Remove(location);
    }
  }

  void RenderPrompt() {
    UIPrompt closest = null;
    float closestDistance = 0;
    foreach (KeyValuePair<Transform, UIPrompt> prompt in prompts) {
      float distance = Vector3.Distance(player.position, prompt.Key.position);
      if (closest == null || distance < closestDistance) {
        closest = prompt.Value;
        closestDistance = distance;
      }
    }
    if (currentPrompt != closest) {
      if (currentPrompt != null) {
        currentPrompt.DestroyCanvas();
        currentPrompt = null;
      }
      if (closest != null) {
        currentPrompt = closest;
        currentPrompt.CreateCanvas();
      }
    }
  }

}
