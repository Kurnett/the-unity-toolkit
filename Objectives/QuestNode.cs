using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNode : Node {
  // Quest Data
  public string text;

  protected override void AddOption() {
    ConversationOption newOption = (ConversationOption)ScriptableObject.CreateInstance(typeof(ConversationOption));
    newOption.Construct((NodeGraph)graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}