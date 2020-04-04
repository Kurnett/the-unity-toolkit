using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNode : Node {
  // Quest Data
  public string text;

  protected override void DrawHeader() {
    bool diff = false;
    GUILayout.Box("", GUIStyle.none);
    GUILayout.Label("Quest Log");
    bool startNew = EditorGUILayout.ToggleLeft("Start", start);
    string textNew = EditorGUILayout.TextArea(text, GUILayout.Height(90));
    if (start != startNew) {
      start = startNew;
      diff = true;
    }
    if (text != textNew) {
      text = textNew;
      diff = true;
    }
    if (diff) SaveGraph(graph);
  }

  protected override void DrawOptionControlsCenter(int i) {
    QuestOption option = (QuestOption)options[i];
    option.id = EditorGUILayout.TextArea(option.id);
  }

  protected override void AddOption() {
    QuestOption newOption = (QuestOption)ScriptableObject.CreateInstance(typeof(QuestOption));
    newOption.Construct((NodeGraph)graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}