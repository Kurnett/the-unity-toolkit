using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : Node<DialogueOption, Flag> {
  public Speaker speaker;
  public string text;
  public Flag textFlag;
  public float length;
  public bool autoProceed;

  public string title = "";
}