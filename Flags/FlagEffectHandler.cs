using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlagEffectHandler {
  public Flag flag;
  public UnityEvent actions;

  public void HandleFlag(Flag flag) {
    if (flag == this.flag) {
      actions.Invoke();
    }
  }
}
