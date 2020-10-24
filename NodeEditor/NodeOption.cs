using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeOption : ScriptableObject {
  public int next = -1;
  public Rect rect;

  public override string ToString() {
    return "next: " + next.ToString();
  }
}