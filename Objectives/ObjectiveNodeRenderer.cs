using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectiveNodeRenderer : NodeRenderer<ObjectiveGraph, ObjectiveNode, ObjectiveOption> {

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
