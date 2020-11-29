using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class NodeSideEffectHandler {
  public NodeSideEffect effect;
  public UnityEvent actions;

  public void HandleEffect(NodeSideEffect effect) {
    if (effect == this.effect) {
      actions.Invoke();
    }
  }
}
