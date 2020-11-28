using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectiveNodeRenderer : NodeRenderer<ObjectiveGraph, ObjectiveNode, ObjectiveOption, NodeSideEffect> {

  protected override void DrawHeader(ObjectiveNode node) {
    bool diff = false;
    GUILayout.Box("", GUIStyle.none);
    GUILayout.Label("Objectives");
    bool startNew = EditorGUILayout.ToggleLeft("Start", node.start);
    string textNew = EditorGUILayout.TextArea(node.text, GUILayout.Height(90));
    if (node.start != startNew) {
      node.start = startNew;
      diff = true;
    }

    // Check if the objective node has a default next node.

    if (node.defaultOption == null || node.defaultOption.next == -1) {
      if (GUILayout.Button("+")) {
        OnClickOption(node.defaultOption);
      }
    } else {
      if (GUILayout.Button("-")) {
        OnRemoveConnection(node.defaultOption);
      }
    }
    
    if (node.text != textNew) {
      node.text = textNew;
      diff = true;
    }
    if (diff) SaveGraph(graph);
  }

  protected override void DrawOptionControlsCenter(ObjectiveNode node, int i) {
    ObjectiveOption option = (ObjectiveOption)node.options[i];
    option.id = EditorGUILayout.TextArea(option.id);
  }

}
