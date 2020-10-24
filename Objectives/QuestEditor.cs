using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestEditor : NodeEditor<Questline, QuestNode, QuestlineRenderer, QuestOption> {

  [MenuItem("Window/Quest Editor")]
  private static void OpenWindow() {
    QuestEditor window = GetWindow<QuestEditor>();
    window.titleContent = new GUIContent("Quest Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select a questline to get started";
  }

  protected override void OnClickAddNode(Vector2 mousePosition) {
    selectedGraph.AddNode(mousePosition);
    SaveGraph(selectedGraph);
  }

  private void DrawConnectionLine(Event e) {
    if (selectedOption != null) {
      Handles.DrawBezier(
        selectedOption.rect.center,
        e.mousePosition,
        selectedOption.rect.center - Vector2.left * 50f,
        e.mousePosition + Vector2.left * 50f,
        Color.white,
        null,
        2f
      );
      GUI.changed = true;
    }
  }

  protected override void SaveGraph(NodeGraph<QuestNode, QuestOption> graph) {
    EditorUtility.SetDirty((Questline)graph);
    AssetDatabase.SaveAssets();
  }
}
