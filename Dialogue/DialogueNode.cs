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

  public string title = "";
}