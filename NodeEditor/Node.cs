using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*

Remove methods
- Draw
- DrawHandles
- Draw...etc
- AddOption
- RemoveOption
- MoveOption

*/

public abstract class Node : ScriptableObject {
  public int id;
  public bool start;
  public List<NodeOption> options = new List<NodeOption>();
  public NodeOption defaultOption;

  // Editor Data
  public Rect rect;
  public Rect containerRect;
  public Rect[] optionRects;

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

  public void MoveOption(NodeOption option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  public virtual void AddOption() {
    NodeOption newOption = (NodeOption)ScriptableObject.CreateInstance(typeof(NodeOption));
    newOption.Construct(graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }

  public virtual void RemoveOption(NodeOption option) {
    AssetDatabase.RemoveObjectFromAsset(option);
    options.Remove(option);
  }
}