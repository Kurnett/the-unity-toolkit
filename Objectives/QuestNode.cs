using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNode : Node {
  // Quest Data
  public string text;

  public override void AddOption() {
    QuestOption newOption = (QuestOption)ScriptableObject.CreateInstance(typeof(QuestOption));
    // newOption.Construct(SaveGraph);
    // AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}