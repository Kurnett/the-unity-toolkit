using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Implement UIPromptTrigger as a class inheriting from a more generic parent class.
public class UIPromptTrigger : MonoBehaviour {

  UIPromptManager manager;
  public UIPrompt prompt;
  public Transform location;

  void Start() {
    manager = UIPromptManager.Instance;
    if (location == null) { location = transform; }
  }

  void OnTriggerEnter() {
    manager.AddPrompt(prompt, location);
  }

  void OnTriggerExit() {
    manager.RemovePrompt(prompt, location);
  }

  void OnDestroy() {
    manager.RemovePrompt(prompt, location);
  }

}
