using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeRenderer<T> where T : Node {

  protected T node;

  public NodeRenderer(Node nodeInit) {
    node = (T)nodeInit;
  }

  protected virtual void DrawHeader() {
    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
  }

  protected virtual void DrawOptions() {
    for (int i = 0; i < node.options.Count; i++) {
      DrawOption(i);
    }
  }

  protected virtual void DrawOption(int i) {
    EditorGUILayout.BeginHorizontal();
    DrawOptionControlsLeft(i);
    DrawOptionControlsCenter(i);
    DrawOptionControlsRight(i);
    EditorGUILayout.EndHorizontal();
  }

  protected virtual void DrawOptionControlsLeft(int i) {
    NodeOption option = node.options[i];
    EditorGUILayout.BeginVertical();
    if (GUILayout.Button("↑", GUILayout.Width(30))) { node.MoveOption(option, -1); }
    if (GUILayout.Button("↓", GUILayout.Width(30))) { node.MoveOption(option, 1); }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawOptionControlsCenter(int i) { }

  protected virtual void DrawOptionControlsRight(int i) {
    NodeOption option = node.options[i];
    if (GUILayout.Button("R", GUILayout.Width(30))) { node.RemoveOption(option); }
    node.optionRects[i] = EditorGUILayout.BeginVertical();
    if (option.next == -1) {
      if (GUILayout.Button("+", GUILayout.Width(30))) { node.OnClickOption(option); }
    } else {
      if (GUILayout.Button("-", GUILayout.Width(30))) { option.RemoveConnection(); }
    }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawAddOption() {
    if (GUILayout.Button("Add Option")) {
      node.AddOption();
      GUI.changed = true;
    }
  }

  protected virtual void DrawHandles() {
    for (int i = 0; i < node.options.Count; i++) {
      NodeOption option = (NodeOption)node.options[i];
      Node nextNode = node.graph.GetNodeById(option.next);
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

  public void DrawNode(T node) {
    EditorStyles.textField.wordWrap = true;
    node.optionRects = new Rect[node.options.Count];

    GUILayout.BeginArea(new Rect(node.rect.x, node.rect.y, 250f, Screen.height * 3));
    node.containerRect = (Rect)EditorGUILayout.BeginVertical("Box");
    DrawHeader();
    DrawOptions();
    DrawAddOption();
    GUILayout.EndVertical();
    GUILayout.EndArea();
    DrawHandles();
  }
}
