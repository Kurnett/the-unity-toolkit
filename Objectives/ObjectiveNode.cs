using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectiveNode : Node<ObjectiveOption> {
  public string text;

  public override void AddOption() {
    ObjectiveOption newOption = (ObjectiveOption)ScriptableObject.CreateInstance(typeof(ObjectiveOption));
    options.Add(newOption);
  }
}