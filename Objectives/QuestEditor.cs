using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestEditor : NodeEditor<QuestlineRenderer, QuestNodeRenderer, Questline, QuestNode, QuestOption> {

  [MenuItem("Window/Quest Editor")]
  private static void OpenWindow() {
    QuestEditor window = GetWindow<QuestEditor>();
    window.titleContent = new GUIContent("Quest Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select a questline to get started";
  }
}
