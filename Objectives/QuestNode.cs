using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNode : Node<QuestOption> {
  public string text;

  public override void AddOption() {
    QuestOption newOption = (QuestOption)ScriptableObject.CreateInstance(typeof(QuestOption));
    options.Add(newOption);
  }
}