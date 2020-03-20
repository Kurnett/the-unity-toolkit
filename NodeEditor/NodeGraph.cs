using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Nodes/Graph")]
public class NodeGraph : ScriptableObject {
  public int id;
  new public string name;
  public List<Node> nodes = new List<Node>();
  public Node startNode;

  public int GenerateUniqueId() {
    bool idFound = false;
    int i = 0;
    while (!idFound) {
      if (GetNodeById(i) == null) {
        return i;
      }
      i++;
    }
    return -1;
  }

  public int GetStartNodeID() {
    foreach (Node node in nodes) {
      if (node.start) {
        return node.id;
      }
    }
    return -1;
  }

  public Node GetNodeById(int id) {
    if (id != -1) {
      Node node = nodes.Find(nodeInit => nodeInit.id == id);
      if (node != null) {
        return node;
      }
      ClearInvalidOptionIDs(id);
    }
    return null;
  }

  public void ClearInvalidOptionIDs(int id) {
    foreach (Node node in nodes) {
      if (node.defaultOption.next == id) {
        node.defaultOption.next = -1;
      }
      foreach (NodeOption option in node.options) {
        if (option.next == id) {
          option.next = -1;
        }
      }
    }
  }

  public void SetStartNode(Node startNode) {
    foreach (Node node in nodes) {
      if (startNode == node) {
        node.start = true;
      } else {
        node.start = false;
      }
    }
  }
}

[System.Serializable]
public class Node {
  public int id;
  public bool start;
  public List<NodeOption> options = new List<NodeOption>();
  public NodeOption defaultOption;

  // Editor Data
  public Rect rect;
  public Rect containerRect;

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

  public Node(
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

  // TODO: Split Draw method into plug-n-play methods for different parts of the UI (primarily the option list).
  
  public void Draw() {
    EditorStyles.textField.wordWrap = true;

    GUILayout.BeginArea(new Rect(rect.x, rect.y, 250f, Screen.height * 3));
    containerRect = (Rect)EditorGUILayout.BeginVertical("Box");

    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);

    for (int i = 0; i < options.Count; i++) {
      NodeOption option = (NodeOption)options[i];
      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.BeginVertical();
      if (GUILayout.Button("↑", GUILayout.Width(30))) { MoveOption(option, -1); }
      if (GUILayout.Button("↓", GUILayout.Width(30))) { MoveOption(option, 1); }
      EditorGUILayout.EndVertical();

      if (GUILayout.Button("R", GUILayout.Width(30))) { options.Remove(option); }
      if (option.next == -1) {
        if (GUILayout.Button("+", GUILayout.Width(30))) { OnClickOption(option); }
      } else {
        if (GUILayout.Button("-", GUILayout.Width(30))) { option.RemoveConnection(); }
      }

      EditorGUILayout.EndHorizontal();
    }

    if (GUILayout.Button("Add Response")) {
      AddOption();
      GUI.changed = true;
    }

    GUILayout.EndVertical();
    GUILayout.EndArea();

    // TODO: Figure out a way to position handles correctly with the new GUI system.
    for (int i = 0; i < options.Count; i++) {
      NodeOption option = (NodeOption)options[i];
      Node nextNode = graph.GetNodeById(option.next);
      Rect addRect = new Rect(rect.x + 130f, rect.y, 30f, 30f);
      option.rect = addRect;

      if (nextNode != null) {
        Handles.DrawBezier(
          addRect.center,
          nextNode.rect.position,
          addRect.center - Vector2.left * 50f,
          nextNode.rect.position + Vector2.left * 50f,
          Color.white,
          null,
          2f
        );
      }
    }

    // TODO: Add save checking back into default graph behavior.

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

  private void ProcessContextMenu() {
    GenericMenu genericMenu = new GenericMenu();
    genericMenu.AddItem(new GUIContent("Remove conversation node"), false, OnClickRemoveNode);
    genericMenu.ShowAsContext();
  }

  private void OnClickRemoveNode() {
    if (OnRemoveNode != null) {
      OnRemoveNode(this);
    }
  }

  private void MoveOption(NodeOption option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  private void AddOption() {
    NodeOption newOption = new NodeOption(graph, SaveGraph);
    options.Add(newOption);
  }
}

[System.Serializable]
public class NodeOption {
  public int next;
  public Rect rect;

  // Needs initialization
  public NodeGraph graph;
  private Action<NodeGraph> SaveGraph;

  public NodeOption() {
    this.next = -1;
    this.graph = null;
  }

  public NodeOption(
    NodeGraph graph,
    Action<NodeGraph> SaveGraph
  ) {
    this.next = -1;
    this.graph = graph;
    this.SaveGraph = SaveGraph;
  }

  public void Initialize(
    NodeGraph graph,
    Action<NodeGraph> SaveGraph
  ) {
    this.graph = graph;
    this.SaveGraph = SaveGraph;
  }

  public void CreateConnection(Node node) {
    this.next = node.id;
    SaveGraph(graph);
  }

  public void RemoveConnection() {
    this.next = -1;
    SaveGraph(graph);
  }
}