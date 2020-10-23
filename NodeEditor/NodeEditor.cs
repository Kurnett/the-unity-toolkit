﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: Refactor save system to follow standard Unity controls (a.k.a use ctrl-s to save).

/*

New methods
- Draw
- DrawNode
- DrawNodeConnection

*/

public abstract class NodeEditor<T, J> : EditorWindow where T : NodeGraph where J : Node {

  protected T selectedGraph;
  protected NodeGraphRenderer<T, J> graphRenderer;
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

    ProcessNodeEvents(Event.current);
    ProcessEventsNodeGraph(Event.current);
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
        graphRenderer = new NodeGraphRenderer<T, J>();
        InitializeNodeGraph();
        graphRenderer.DrawNodeGraph(selectedGraph);
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
          selectedGraph,
          OnClickOption,
          OnClickNode,
          OnClickRemoveNode,
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
        if (e.button == 1) {
          ProcessContextMenu(e.mousePosition);
        }
        break;
      case EventType.MouseDrag:
        if (e.button == 0 && !contextMenuOpen && !graphSelectMenuOpen) {
          OnDrag(e.delta);
        }
        break;
    }
  }

  private void ProcessNodeEvents(Event e) {
    if (selectedGraph != null && selectedGraph.nodes != null) {
      foreach (Node node in selectedGraph.nodes) {
        bool guiChanged = node.ProcessEvents(e);
        if (guiChanged) { GUI.changed = true; }
      }
    }
  }

  private void ProcessContextMenu(Vector2 mousePosition) {
    GenericMenu genericMenu = new GenericMenu();
    genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
    genericMenu.ShowAsContext();
    contextMenuOpen = true;
  }

  private void OnDrag(Vector2 delta) {
    drag = delta;
    if (selectedGraph != null && selectedGraph.nodes != null) {
      foreach (Node node in selectedGraph.nodes) {
        node.Drag(delta);
      }
    }
    GUI.changed = true;
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

  protected void CreateConnection(NodeOption option, Node node) {
    option.CreateConnection(node);
    SaveGraph(selectedGraph);
  }

  protected void ClearConnectionSelection() {
    selectedOption = null;
  }

  protected virtual void SaveGraph(NodeGraph graph) {
    EditorUtility.SetDirty(graph);
    AssetDatabase.SaveAssets();
  }
}
