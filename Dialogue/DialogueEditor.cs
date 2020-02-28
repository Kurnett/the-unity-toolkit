using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: Restyle "select conversation" menu.
// TODO: Add default screen for when no conversation is selected.
// TODO: Add ability to create new conversation scriptable objects in a default location.
// TODO: Create blank starting point node when a conversation is created.
// TODO: Add ability to delete conversations.

public class DialogueEditor : EditorWindow {

  private Conversation selectedConversation;
  [System.NonSerialized]
  private ConversationOption selectedOption;

  private GUIStyle nodeStyle;
  private GUIStyle selectedNodeStyle;

  private Vector2 offset;
  private Vector2 drag;

  [MenuItem("Window/Dialogue Editor")]
  private static void OpenWindow() {
    DialogueEditor window = GetWindow<DialogueEditor>();
    window.titleContent = new GUIContent("Dialogue Editor");
  }

  private void OnEnable() {
    nodeStyle = new GUIStyle();
    nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
    nodeStyle.border = new RectOffset(12, 12, 12, 12);

    selectedNodeStyle = new GUIStyle();
    selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
    selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
  }

  private void OnGUI() {
    DrawGrid(20, 0.2f, Color.gray);
    DrawGrid(100, 0.4f, Color.gray);

    DrawNodes();
    // DrawConnections();
    DrawConnectionLine(Event.current);

    ProcessNodeEvents(Event.current);
    ProcessEvents(Event.current);

    DrawLoadConversation();

    if (GUI.changed) Repaint();
  }

  private void DrawLoadConversation() {
    if (GUI.Button(new Rect(50, 50, 140, 30), "Select Conversation")) {
      OpenConversationPicker();
    }
    EditorGUI.SelectableLabel(new Rect(50, 100, 200, 200), selectedConversation != null ? selectedConversation.name : "");
  }

  private void OpenConversationPicker() {
    EditorGUIUtility.ShowObjectPicker<Conversation>(null, false, "", 0);
  }

  private void SelectConversation() {
    if (EditorGUIUtility.GetObjectPickerObject() is Conversation) {
      selectedConversation = EditorGUIUtility.GetObjectPickerObject() as Conversation;
      if (selectedConversation && selectedConversation.dialogue != null) {
        selectedOption = null;
        foreach (ConversationNode node in selectedConversation.dialogue) {
          node.Initialize(
            selectedConversation,
            nodeStyle,
            selectedNodeStyle,
            OnClickOption,
            OnClickNode,
            OnClickRemoveNode,
            SaveConversation);
        }
      }
      DrawNodes();
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

  private void DrawNodes() {
    if (selectedConversation != null && selectedConversation.dialogue != null) {
      foreach (ConversationNode node in selectedConversation.dialogue) {
        node.Draw();
      }
    }
  }

  // private void DrawConnections() {
  //   if (selectedConversation.dialogue != null) {
  //     foreach (ConversationNode node in selectedConversation.dialogue) {
  //       if (node.outPoint != null && node.outPoint.connection != null)
  //         node.outPoint.connection.Draw();
  //     }
  //   }
  // }

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
    drag = Vector2.zero;
    switch (e.type) {
      case EventType.MouseDown:
        if (e.button == 1) {
          ProcessContextMenu(e.mousePosition);
        }
        break;
      case EventType.MouseDrag:
        if (e.button == 0) {
          OnDrag(e.delta);
        }
        break;
      case EventType.ExecuteCommand:
        // TODO: Add handling for other events of type ExecuteCommand in other contexts.
        SelectConversation();
        break;
    }
  }

  private void ProcessNodeEvents(Event e) {
    if (selectedConversation != null && selectedConversation.dialogue != null) {
      foreach (ConversationNode node in selectedConversation.dialogue) {
        bool guiChanged = node.ProcessEvents(e);
        if (guiChanged) { GUI.changed = true; }
      }
    }
  }

  private void ProcessContextMenu(Vector2 mousePosition) {
    GenericMenu genericMenu = new GenericMenu();
    genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
    genericMenu.ShowAsContext();
  }

  private void OnDrag(Vector2 delta) {
    drag = delta;
    if (selectedConversation != null && selectedConversation.dialogue != null) {
      foreach (ConversationNode node in selectedConversation.dialogue) {
        node.Drag(delta);
      }
    }
    GUI.changed = true;
  }

  private void OnClickAddNode(Vector2 mousePosition) {
    if (selectedConversation != null && selectedConversation.dialogue == null) { selectedConversation.dialogue = new List<ConversationNode>(); }
    selectedConversation.dialogue.Add(new ConversationNode(
      selectedConversation.GenerateUniqueId(),
      mousePosition,
      nodeStyle,
      selectedNodeStyle,
      selectedConversation,
      OnClickOption,
      OnClickNode,
      OnClickRemoveNode,
      SaveConversation
    ));
    SaveConversation(selectedConversation);
  }

  private void OnClickRemoveNode(ConversationNode node) {
    selectedConversation.dialogue.Remove(node);
    SaveConversation(selectedConversation);
  }

  private void OnClickOption(ConversationOption option) {
    if (selectedOption != null) {
      ClearConnectionSelection();
    } else {
      selectedOption = option;
    }
  }

  private void OnClickNode(ConversationNode node) {
    if (selectedOption != null) {
      CreateConnection(selectedOption, node);
      ClearConnectionSelection();
    }
  }

  private void CreateConnection(ConversationOption option, ConversationNode node) {
    option.CreateConnection(node);
    SaveConversation(selectedConversation);
  }

  private void ClearConnectionSelection() {
    selectedOption = null;
  }

  private void SaveConversation(Conversation conversation) {
    EditorUtility.SetDirty(conversation);
    AssetDatabase.SaveAssets();
    Debug.Log("SAVED");
  }
}
