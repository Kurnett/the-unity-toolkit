using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*

- Centralize saving into a single method in NodeEditor.

*/

public abstract class NodeEditor<
  GRAPH_RENDERER,
  NODE_RENDERER,
  NODE_GRAPH,
  NODE,
  NODE_OPTION
> : EditorWindow
  where GRAPH_RENDERER : NodeGraphRenderer<NODE_GRAPH, NODE, NODE_OPTION, NODE_RENDERER>, new()
  where NODE_RENDERER : NodeRenderer<NODE_GRAPH, NODE, NODE_OPTION>, new()
  where NODE_GRAPH : NodeGraph<NODE, NODE_OPTION>
  where NODE : Node<NODE_OPTION>
  where NODE_OPTION : NodeOption {

  protected NODE_GRAPH selectedGraph;
  protected NODE selectedNode;
  protected GRAPH_RENDERER graphRenderer;
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

    DrawNodeGraph();
    DrawConnectionLine(Event.current);

    ProcessEventsNodeGraph(Event.current);
  }

  protected virtual void CheckAndInitializeRenderer() {
    if (graphRenderer == null) {
      graphRenderer = new GRAPH_RENDERER();
    }
  }

  protected virtual void DrawNodeGraph() {
    CheckAndInitializeRenderer();
    graphRenderer.DrawNodeGraph(selectedGraph, OnClickRemoveNode, OnClickRemoveConnection, OnClickOption, SaveGraph);
  }

  private void RenderNodeGraphSelectionGUI() {
    GUILayout.BeginArea(new Rect(18, 18, 196, 48));
    EditorGUILayout.BeginVertical("Box");
    if (!selectedGraph) {
      NODE_GRAPH newGraph = (NODE_GRAPH)EditorGUILayout.ObjectField(selectedGraph, typeof(NODE_GRAPH), false);
      EditorGUILayout.LabelField("Select Node Graph");
      if (newGraph != selectedGraph) {
        graphSelectMenuOpen = true;
        selectedGraph = newGraph as NODE_GRAPH;
        InitializeNodeGraph();
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

  private void ProcessEventsNodeGraph(Event e) {
    drag = Vector2.zero;
    switch (e.type) {
      case EventType.MouseDown:
        contextMenuOpen = false;
        graphSelectMenuOpen = false;
        if (e.button == 0) {
          selectedNode = (NODE)GetNodeAtPoint(e.mousePosition.x, e.mousePosition.y);
          if (selectedNode != null) {
            OnClickNode(selectedNode);
          }
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
          foreach (NODE node in selectedGraph.nodes) {
            Drag(node, e.delta);
          }
        }
        e.Use();
        break;
    }
  }

  private void ProcessContextMenu(Vector2 mousePosition) {
    NODE node = GetNodeAtPoint(mousePosition.x, mousePosition.y);
    GenericMenu genericMenu = new GenericMenu();
    if (node == null) {
      genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
      genericMenu.ShowAsContext();
    } else {
      genericMenu.AddItem(new GUIContent("Remove node"), false, () => OnClickRemoveNode(node));
      genericMenu.ShowAsContext();
    }
    contextMenuOpen = true;
  }

  protected virtual void OnClickAddNode(Vector2 mousePosition) {
    selectedGraph.AddNode(mousePosition);
    SaveGraph(selectedGraph);
  }

  protected void OnClickRemoveNode(NODE node) {
    selectedGraph.RemoveNode(node);
    SaveGraph(selectedGraph);
  }

  protected void OnClickRemoveConnection(NODE_OPTION option) {
    option.next = -1;
    SaveGraph(selectedGraph);
  }

  protected void OnClickOption(NodeOption option) {
    if (selectedOption != null) {
      ClearConnectionSelection();
    } else {
      selectedOption = option;
    }
  }

  protected void OnClickNode(NODE node) {
    if (selectedOption != null) {
      CreateConnection(selectedOption, node);
      ClearConnectionSelection();
    }
  }

  protected void ClearConnectionSelection() {
    selectedOption = null;
  }

  protected virtual void SaveGraph(NodeGraph<NODE, NODE_OPTION> graph) {
    // Repair any broken default nodes before saving.
    foreach (NODE node in graph.nodes) {
      if (node.defaultOption == null) {
        NODE_OPTION newOption = (NODE_OPTION)ScriptableObject.CreateInstance(typeof(NODE_OPTION));
        AssetDatabase.AddObjectToAsset(newOption, this);
        node.defaultOption = newOption;
      }
    }
    EditorUtility.SetDirty(graph);
    AssetDatabase.SaveAssets();
  }

  public void Drag(NODE node, Vector2 delta) {
    node.rect.position += delta;
  }

  public NODE GetNodeAtPoint(float x, float y) {
    foreach (NODE node in selectedGraph.nodes) {
      if (node.PointIsInBoundingBox(x, y)) {
        return node;
      }
    }
    return null;
  }

  public void CreateConnection(NodeOption option, NODE node) {
    option.next = node.id;
    SaveGraph(selectedGraph);
  }

  public void RemoveConnection(NodeOption option) {
    option.next = -1;
    SaveGraph(selectedGraph);
  }
}
