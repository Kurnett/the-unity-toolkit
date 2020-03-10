using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Replace setting player with a more extensible system.
public class UIPromptPlayer : MonoBehaviour {
  void Start() {
    UIPromptManager.Instance.SetPlayer(transform);
  }
}
