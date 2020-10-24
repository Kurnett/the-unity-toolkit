using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeRenderer<T, J> where T : NodeGraph where J : Node {

  // protected K editor;
  protected T graph;

  public NodeRenderer(T graphInit) {
    // editor = editorInit;
    graph = graphInit;
  }

  protected virtual void DrawHeader(J node) {
    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
  }

  protected virtual void DrawOptions(J node) {
    for (int i = 0; i < node.options.Count; i++) {
      DrawOption(node, i);
    }
  }

  protected virtual void DrawOption(J node, int i) {
    EditorGUILayout.BeginHorizontal();
    DrawOptionControlsLeft(node, i);
    DrawOptionControlsCenter(node, i);
    DrawOptionControlsRight(node, i);
    EditorGUILayout.EndHorizontal();
  }

  protected virtual void DrawOptionControlsLeft(J node, int i) {
    NodeOption option = node.options[i];
    EditorGUILayout.BeginVertical();
    if (GUILayout.Button("↑", GUILayout.Width(30))) { node.MoveOption(option, -1); }
    if (GUILayout.Button("↓", GUILayout.Width(30))) { node.MoveOption(option, 1); }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawOptionControlsCenter(J node, int i) { }

  protected virtual void DrawOptionControlsRight(J node, int i) {
    NodeOption option = node.options[i];
    if (GUILayout.Button("R", GUILayout.Width(30))) { node.RemoveOption(option); }
    node.optionRects[i] = EditorGUILayout.BeginVertical();
    if (option.next == -1) {
      if (GUILayout.Button("+", GUILayout.Width(30))) { node.OnClickOption(option); }
    } else {
      // if (GUILayout.Button("-", GUILayout.Width(30))) { editor.RemoveConnection(option); }
    }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawAddOption(J node) {
    if (GUILayout.Button("Add Option")) {
      node.AddOption();
      GUI.changed = true;
    }
  }

  protected virtual void DrawHandles(J node) {
    for (int i = 0; i < node.options.Count; i++) {
      NodeOption option = (NodeOption)node.options[i];
      Node nextNode = graph.GetNodeById(option.next);
      Rect addRect = new Rect(node.rect.x + 130f, node.rect.y, 30f, 30f);
      option.rect = addRect;

      if (nextNode != null) {
        Vector2 handlePos = new Vector2(node.rect.x + node.optionRects[i].xMax, node.rect.y + node.optionRects[i].y);
        Handles.DrawBezier(
          handlePos,
          nextNode.rect.position,
          handlePos - Vector2.left * 50f,
          nextNode.rect.position + Vector2.left * 50f,
          Color.white,
          null,
          2f
        );
      }
    }
  }

  public void DrawNode(J node) {
    EditorStyles.textField.wordWrap = true;
    node.optionRects = new Rect[node.options.Count];

    GUILayout.BeginArea(new Rect(node.rect.x, node.rect.y, 250f, Screen.height * 3));
    node.containerRect = (Rect)EditorGUILayout.BeginVertical("Box");
    DrawHeader(node);
    DrawOptions(node);
    DrawAddOption(node);
    GUILayout.EndVertical();
    GUILayout.EndArea();
    DrawHandles(node);
  }
}
