using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPromptPlayer : MonoBehaviour {
  void Start() {
    UIPromptManager.Instance.SetPlayer(transform);
  }
}
