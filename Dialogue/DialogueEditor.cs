using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : NodeEditor<Conversation, ConversationNode> {

  [MenuItem("Window/Dialogue Editor")]
  private static void OpenWindow() {
    DialogueEditor window = GetWindow<DialogueEditor>();
    window.titleContent = new GUIContent("Dialogue Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select a conversation to get started";
  }

  protected override void OnClickAddNode(Vector2 mousePosition) {
    selectedGraph.AddNode(
      mousePosition,
      OnClickOption,
      OnClickNode,
      OnClickRemoveNode,
      SaveGraph
    );
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

  protected override void SaveGraph(NodeGraph graph) {
    EditorUtility.SetDirty((Conversation)graph);
    AssetDatabase.SaveAssets();
  }

  override protected void DrawNodeGraph() {
    CheckAndInitializeRenderer();
    graphRenderer.DrawNodeGraph(selectedGraph);
  }
}
