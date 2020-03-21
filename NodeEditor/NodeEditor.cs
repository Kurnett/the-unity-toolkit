using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class NodeEditor<T> : EditorWindow where T : NodeGraph {

  protected T selectedGraph;
  [System.NonSerialized]
  protected NodeOption selectedOption;

  protected GUIStyle centerText;

  protected Vector2 offset;
  protected Vector2 drag;

  protected bool contextMenuOpen;
  protected bool graphSelectMenuOpen;

  // [UnityEditor.Callbacks.DidReloadScripts]
  // private static void OnScriptReload() {
  //   List<NodeEditor<T>> nodeEditors = new List<NodeEditor<T>>();
  //   foreach (NodeEditor<T> editor in Resources.FindObjectsOfTypeAll(typeof(NodeEditor<T>)) as NodeEditor<T>[]) {
  //     editor.InitializeNodeGraph();
  //   }
  // }

  // [MenuItem("Window/Node Editor")]
  // private static void OpenWindow() {
  //   NodeEditor<T> window = GetWindow<NodeEditor<T>>();
  //   window.titleContent = new GUIContent("Node Editor");
  // }

  private void OnGUI() {
    if (selectedGraph == null) {
      RenderNoConversationSelectedGUI();
    } else {
      RenderConversationSelectedGUI();
    }
    RenderConversationSelectionGUI();
    ProcessEvents(Event.current);
    if (GUI.changed) Repaint();
  }

  protected virtual void RenderNoConversationSelectedGUI() {
    centerText = new GUIStyle();
    centerText.alignment = TextAnchor.MiddleCenter;
    EditorGUI.LabelField(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 25, 400, 50), "Select a node graph to get started", centerText);
  }

  private void RenderConversationSelectedGUI() {
    DrawGrid(20, 0.2f, Color.gray);
    DrawGrid(100, 0.4f, Color.gray);

    selectedGraph.Draw();
    DrawConnectionLine(Event.current);

    ProcessNodeEvents(Event.current);
    ProcessEventsConversation(Event.current);
  }

  private void RenderConversationSelectionGUI() {
    GUILayout.BeginArea(new Rect(18, 18, 196, 48));
    EditorGUILayout.BeginVertical("Box");
    if (!selectedGraph) {
      T newGraph = (T)EditorGUILayout.ObjectField(selectedGraph, typeof(T), false);
      EditorGUILayout.LabelField("Select Node Graph");
      if (newGraph != selectedGraph) {
        graphSelectMenuOpen = true;
        selectedGraph = newGraph as T;
        InitializeNodeGraph();
        selectedGraph.Draw();
      }
    } else {
      if (GUILayout.Button("Select New Conv.")) {
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
          SaveGraph<NodeGraph>);
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

  private void ProcessEventsConversation(Event e) {
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
    selectedGraph.nodes.Remove(node);
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

  protected void SaveGraph<U>(U graph) where U : NodeGraph {
    EditorUtility.SetDirty(graph);
    AssetDatabase.SaveAssets();
  }
}
