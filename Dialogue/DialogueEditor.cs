﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : NodeEditor<
  ConversationRenderer,
  ConversationNodeRenderer,
  Conversation,
  ConversationNode,
  ConversationOption
> {

  [MenuItem("Window/Dialogue Editor")]
  private static void OpenWindow() {
    DialogueEditor window = GetWindow<DialogueEditor>();
    window.titleContent = new GUIContent("Dialogue Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select a conversation to get started";
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

  protected override void SaveGraph(NodeGraph<ConversationNode, ConversationOption> graph) {
    EditorUtility.SetDirty((Conversation)graph);
    AssetDatabase.SaveAssets();
  }

  protected override void CheckAndInitializeRenderer() {
    if (graphRenderer == null) {
      graphRenderer = new ConversationRenderer();
    }
  }
}
