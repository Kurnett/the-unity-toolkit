using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Node : ScriptableObject {
  public int id;
  public bool start;
  public List<NodeOption> options = new List<NodeOption>();
  public NodeOption defaultOption;

  // Editor Data
  public Rect rect;
  public Rect containerRect;
  protected Rect[] optionRects;

  public bool isDragged;
  public bool isSelected;

  public float width = 200f;
  public float height = 30f;

  // Needs initialization
  public NodeGraph graph;
  public Action<Node> OnClickNode;
  public Action<Node> OnRemoveNode;
  public Action<NodeOption> OnClickOption;
  public Action<NodeGraph> SaveGraph;

  virtual public void Construct(
    int id,
    Vector2 position,
    NodeGraph graph,
    Action<NodeOption> OnClickOption,
    Action<Node> OnClickNode,
    Action<Node> OnRemoveNode,
    Action<NodeGraph> SaveGraph
  ) {
    this.id = id;
    rect = new Rect(position.x, position.y, width, height);
    Initialize(graph, OnClickOption, OnClickNode, OnRemoveNode, SaveGraph);
  }

  public void Initialize(
    NodeGraph graph,
    Action<NodeOption> OnClickOption,
    Action<Node> OnClickNode,
    Action<Node> OnRemoveNode,
    Action<NodeGraph> SaveGraph
  ) {
    this.graph = graph;
    this.OnClickOption = OnClickOption;
    this.OnClickNode = OnClickNode;
    this.OnRemoveNode = OnRemoveNode;
    this.SaveGraph = SaveGraph;
    for (int i = 0; i < options.Count; i++) {
      options[i].Initialize(graph, SaveGraph);
    }
  }

  public void Drag(Vector2 delta) {
    rect.position += delta;
  }

  virtual public void Draw() {
    EditorStyles.textField.wordWrap = true;
    optionRects = new Rect[options.Count];

    GUILayout.BeginArea(new Rect(rect.x, rect.y, 250f, Screen.height * 3));
    containerRect = (Rect)EditorGUILayout.BeginVertical("Box");
    DrawHeader();
    DrawOptions();
    DrawAddOption();
    GUILayout.EndVertical();
    GUILayout.EndArea();
    DrawHandles();
  }

  protected virtual void DrawHeader() {
    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
  }

  protected virtual void DrawOptions() {
    for (int i = 0; i < options.Count; i++) {
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
    NodeOption option = options[i];
    EditorGUILayout.BeginVertical();
    if (GUILayout.Button("↑", GUILayout.Width(30))) { MoveOption(option, -1); }
    if (GUILayout.Button("↓", GUILayout.Width(30))) { MoveOption(option, 1); }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawOptionControlsCenter(int i) { }

  protected virtual void DrawOptionControlsRight(int i) {
    NodeOption option = options[i];
    if (GUILayout.Button("R", GUILayout.Width(30))) { RemoveOption(option); }
    optionRects[i] = EditorGUILayout.BeginVertical();
    if (option.next == -1) {
      if (GUILayout.Button("+", GUILayout.Width(30))) { OnClickOption(option); }
    } else {
      if (GUILayout.Button("-", GUILayout.Width(30))) { option.RemoveConnection(); }
    }
    EditorGUILayout.EndVertical();
  }

  protected virtual void DrawAddOption() {
    if (GUILayout.Button("Add Option")) {
      AddOption();
      GUI.changed = true;
    }
  }

  protected virtual void DrawHandles() {
    for (int i = 0; i < options.Count; i++) {
      NodeOption option = (NodeOption)options[i];
      Node nextNode = graph.GetNodeById(option.next);
      Rect addRect = new Rect(rect.x + 130f, rect.y, 30f, 30f);
      option.rect = addRect;

      if (nextNode != null) {
        Vector2 handlePos = new Vector2(rect.x + optionRects[i].xMax, rect.y + optionRects[i].y);
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

  public bool ProcessEvents(Event e) {
    Rect dragRect = new Rect(rect.x + containerRect.x, rect.y + containerRect.y, containerRect.width, containerRect.height);
    switch (e.type) {
      case EventType.MouseDown:
        if (e.button == 0) {
          if (dragRect.Contains(e.mousePosition)) {
            isDragged = true;
            GUI.changed = true;
            isSelected = true;
            OnClickNode(this);
          } else {
            GUI.changed = true;
            isSelected = false;
          }
        }

        if (e.button == 1 && dragRect.Contains(e.mousePosition)) {
          ProcessContextMenu();
          e.Use();
        }
        break;
      case EventType.MouseUp:
        isDragged = false;
        break;
      case EventType.MouseDrag:
        if (e.button == 0 && isDragged) {
          Drag(e.delta);
          e.Use();
          return true;
        }
        break;
    }
    return false;
  }

  protected void ProcessContextMenu() {
    GenericMenu genericMenu = new GenericMenu();
    genericMenu.AddItem(new GUIContent("Remove conversation node"), false, OnClickRemoveNode);
    genericMenu.ShowAsContext();
  }

  protected void OnClickRemoveNode() {
    if (OnRemoveNode != null) {
      OnRemoveNode(this);
    }
  }

  protected void MoveOption(NodeOption option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  protected virtual void AddOption() {
    NodeOption newOption = (NodeOption)ScriptableObject.CreateInstance(typeof(NodeOption));
    newOption.Construct(graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }

  protected virtual void RemoveOption(NodeOption option) {
    AssetDatabase.RemoveObjectFromAsset(option);
    options.Remove(option);
  }
}