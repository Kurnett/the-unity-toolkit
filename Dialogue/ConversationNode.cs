using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationNode : Node<ConversationOption> {
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public bool endConversation;

  public string title = "";

  public override void AddOption() {
    ConversationOption newOption = (ConversationOption)ScriptableObject.CreateInstance(typeof(ConversationOption));
    options.Add(newOption);
  }
}