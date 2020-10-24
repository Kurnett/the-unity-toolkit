using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: Refactor save system to follow standard Unity controls (a.k.a use ctrl-s to save).

/*

New methods
- CreateNodeConnection
- RemoveNodeConnection
- AddOption
- RemoveOption
- MoveOption

*/

public abstract class NodeEditor<T, J, K> : EditorWindow where T : NodeGraph where J : Node where K : NodeGraphRenderer<T, J> {

  protected T selectedGraph;
  protected J selectedNode;
  protected K graphRenderer;
  [System.NonSerialized]
  protected NodeOption selectedOption;

  protected GUIStyle centerText;

  protected Vector2 offset;
  protected Vector2 drag;

  protected bool contextMenuOpen;
  protected bool graphSelectMenuOpen;

  private void OnGUI() {
    if (selectedGraph == null) {
      RenderNoNodeGraphSelectedGUI();
    } else {
      RenderNodeGraphSelectedGUI();
    }
    RenderNodeGraphSelectionGUI();
    ProcessEvents(Event.current);
    if (GUI.changed) Repaint();
  }

  protected virtual void RenderNoNodeGraphSelectedGUI() {
    centerText = new GUIStyle();
    centerText.alignment = TextAnchor.MiddleCenter;
    EditorGUI.LabelField(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 25, 400, 50), GetNoSelectionMessage(), centerText);
  }

  protected virtual string GetNoSelectionMessage() {
    return "Select a node graph to get started";
  }

  private void RenderNodeGraphSelectedGUI() {
    DrawGrid(20, 0.2f, Color.gray);
    DrawGrid(100, 0.4f, Color.gray);

    if (graphRenderer != null) {
      graphRenderer.DrawNodeGraph(selectedGraph);
    }
    DrawConnectionLine(Event.current);

    ProcessEventsNodeGraph(Event.current);
  }

  protected virtual void CheckAndInitializeRenderer() {
    if (graphRenderer == null) {
      graphRenderer = (K)new NodeGraphRenderer<T, J>();
    }
  }

  protected virtual void DrawNodeGraph() {
    CheckAndInitializeRenderer();
    graphRenderer.DrawNodeGraph(selectedGraph);
  }

  private void RenderNodeGraphSelectionGUI() {
    GUILayout.BeginArea(new Rect(18, 18, 196, 48));
    EditorGUILayout.BeginVertical("Box");
    if (!selectedGraph) {
      T newGraph = (T)EditorGUILayout.ObjectField(selectedGraph, typeof(T), false);
      EditorGUILayout.LabelField("Select Node Graph");
      if (newGraph != selectedGraph) {
        graphSelectMenuOpen = true;
        selectedGraph = newGraph as T;
        InitializeNodeGraph();
        DrawNodeGraph();
      }
    } else {
      if (GUILayout.Button("Select New Graph")) {
        selectedGraph = null;
      }
      if (selectedGraph != null) EditorGUILayout.LabelField(selectedGraph.name);
    }
    EditorGUILayout.EndVertical();
    GUILayout.EndArea();
  }

  private void InitializeNodeGraph() {
    if (selectedGraph && selectedGraph.nodes != null) {
      selectedOption = null;
      foreach (Node node in selectedGraph.nodes) {
        node.Initialize(
          OnClickOption,
          OnClickNode,
          SaveGraph);
      }
    }
  }

  private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
    int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
    int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

    Handles.BeginGUI();
    Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

    offset += drag * 0.5f;
    Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

    for (int i = 0; i < widthDivs; i++) {
      Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
    }

    for (int j = 0; j < heightDivs; j++) {
      Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
    }

    Handles.color = Color.white;
    Handles.EndGUI();
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

  private void ProcessEvents(Event e) {

  }

  private void ProcessEventsNodeGraph(Event e) {
    drag = Vector2.zero;
    switch (e.type) {
      case EventType.MouseDown:
        contextMenuOpen = false;
        graphSelectMenuOpen = false;
        if (e.button == 0) {
          selectedNode = (J)GetNodeAtPoint(e.mousePosition.x, e.mousePosition.y);
        }
        if (e.button == 1) {
          ProcessContextMenu(e.mousePosition);
        }
        break;
      case EventType.MouseUp:
        selectedNode = null;
        break;
      case EventType.MouseDrag:
        if (selectedNode != null) {
          Drag(selectedNode, e.delta);
        } else {
          offset += e.delta;
          foreach (Node node in selectedGraph.nodes) {
            Drag(node, e.delta);
          }
        }
        e.Use();
        break;
    }
  }

  private void ProcessContextMenu(Vector2 mousePosition) {
    Node node = GetNodeAtPoint(mousePosition.x, mousePosition.y);
    GenericMenu genericMenu = new GenericMenu();
    if (node == null) {
      genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
      genericMenu.ShowAsContext();
    } else {
      genericMenu.AddItem(new GUIContent("Remove conversation node"), false, () => OnClickRemoveNode(node));
      genericMenu.ShowAsContext();
    }
    contextMenuOpen = true;
  }

  protected abstract void OnClickAddNode(Vector2 mousePosition);

  protected void OnClickRemoveNode(Node node) {
    selectedGraph.RemoveNode(node);
    SaveGraph(selectedGraph);
  }

  protected void OnClickOption(NodeOption option) {
    if (selectedOption != null) {
      ClearConnectionSelection();
    } else {
      selectedOption = option;
    }
  }

  protected void OnClickNode(Node node) {
    if (selectedOption != null) {
      CreateConnection(selectedOption, node);
      ClearConnectionSelection();
    }
  }

  protected void ClearConnectionSelection() {
    selectedOption = null;
  }

  protected virtual void SaveGraph(NodeGraph graph) {
    EditorUtility.SetDirty(graph);
    AssetDatabase.SaveAssets();
  }

  public void Drag(Node node, Vector2 delta) {
    node.rect.position += delta;
  }

  public Node GetNodeAtPoint(float x, float y) {
    foreach (Node node in selectedGraph.nodes) {
      if (node.PointIsInBoundingBox(x, y)) {
        return node;
      }
    }
    return null;
  }

  public void CreateConnection(NodeOption option, Node node) {
    option.next = node.id;
    SaveGraph(selectedGraph);
  }

  public void RemoveConnection(NodeOption option) {
    option.next = -1;
    SaveGraph(selectedGraph);
  }
}
