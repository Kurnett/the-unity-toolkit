using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationNode : Node {
  // Conversation Data
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public bool endConversation;

  public string title = "";

  public override void AddOption() {
    ConversationOption newOption = (ConversationOption)ScriptableObject.CreateInstance(typeof(ConversationOption));
    newOption.Construct((NodeGraph)graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}