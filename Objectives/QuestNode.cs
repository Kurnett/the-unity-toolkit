using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNode : Node {
  // Quest Data
  public string text;

  protected override void DrawHeader() {
    GUILayout.Box("", GUIStyle.none);
    GUILayout.Label("Quest Log");
    text = EditorGUILayout.TextArea(text, GUILayout.Height(90));
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