using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : Node<DialogueOption> {
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public bool endDialogue;

  public string title = "";

  public override void AddOption() {
    DialogueOption newOption = (DialogueOption)ScriptableObject.CreateInstance(typeof(DialogueOption));
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }
}