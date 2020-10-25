using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeRenderer<NODE_GRAPH, NODE, NODE_OPTION>
  where NODE_GRAPH : NodeGraph<NODE, NODE_OPTION>
  where NODE : Node<NODE_OPTION>
  where NODE_OPTION : NodeOption {

  // Needs initialization, avoids needing editor reference
  public Action<NODE> OnClickNode;
  public Action<NODE> OnRemoveNode;
  public Action<NODE_OPTION> OnClickOption;
  public Action<NODE_GRAPH> SaveGraph;

  protected NODE_GRAPH graph;

  public NodeRenderer() { }

  public void Initialize(
    NODE_GRAPH graph,
    Action<NODE> OnClickNode,
    Action<NODE> OnRemoveNode,
    Action<NODE_OPTION> OnClickOption,
    Action<NODE_GRAPH> SaveGraph
   ) {
    this.graph = graph;
    this.OnClickNode = OnClickNode;
    this.OnRemoveNode = OnRemoveNode;
    this.OnClickOption = OnClickOption;
    this.SaveGraph = SaveGraph;
  }

  protected virtual void DrawHeader(NODE node) {
    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
  }

  protected virtual void DrawOptions(NODE node) {
    for (int i = 0; i < node.options.Count; i++) {
      DrawOption(node, i);
    }
  }

  protected virtual void DrawOption(NODE node, int i) {
    EditorGUILayout.BeginHorizontal();
    DrawOptionControlsLeft(node, i);
    DrawOptionControlsCenter(node, i);
    DrawOptionControlsRight(node, i);
    EditorGUILayout.EndHorizontal();
  }

  protected virtual void DrawOptionControlsLeft(NODE node, int i) {
    NODE_OPTION option = node.options[i];
    EditorGUILayout.BeginVertical();
    if (GUILayout.Button("↑", GUILayout.Width(30))) { node.MoveOption(option, -1); }
    if (GUILayout.Button("↓", GUILayout.Width(30))) { node.MoveOption(option, 1); }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawOptionControlsCenter(NODE node, int i) { }

  protected virtual void DrawOptionControlsRight(NODE node, int i) {
    NODE_OPTION option = node.options[i];
    if (GUILayout.Button("R", GUILayout.Width(30))) { node.RemoveOption(option); }
    node.optionRects[i] = EditorGUILayout.BeginVertical();
    if (option.next == -1) {
      if (GUILayout.Button("+", GUILayout.Width(30))) { OnClickOption(option); }
    } else {
      // if (GUILayout.Button("-", GUILayout.Width(30))) { editor.RemoveConnection(option); }
    }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawAddOption(NODE node) {
    if (GUILayout.Button("Add Option")) {
      node.AddOption();
      GUI.changed = true;
    }
  }

  protected virtual void DrawHandles(NODE node) {
    for (int i = 0; i < node.options.Count; i++) {
      NODE_OPTION option = (NODE_OPTION)node.options[i];
      NODE nextNode = graph.GetNodeById(option.next);
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

  public void DrawNode(NODE node) {
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
