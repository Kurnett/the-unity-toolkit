using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : NodeEditor<Conversation> {

  // private Conversation selectedConversation;
  // [System.NonSerialized]
  // private ConversationOption selectedOption;

  [MenuItem("Window/Dialogue Editor")]
  private static void OpenWindow() {
    DialogueEditor window = GetWindow<DialogueEditor>();
    window.titleContent = new GUIContent("Dialogue Editor");
  }

  protected override void RenderNoConversationSelectedGUI() {
    centerText = new GUIStyle();
    centerText.alignment = TextAnchor.MiddleCenter;
    EditorGUI.LabelField(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 25, 400, 50), "Select a conversation to get started", centerText);
  }

  protected override void OnClickAddNode(Vector2 mousePosition) {
    if (selectedGraph != null && selectedGraph.nodes == null) { selectedGraph.nodes = new List<Node>(); }
    ConversationNode newNode = (ConversationNode)ScriptableObject.CreateInstance(typeof(ConversationNode));
    newNode.Construct(
      selectedGraph.GenerateUniqueId(),
      mousePosition,
      selectedGraph,
      OnClickOption,
      OnClickNode,
      OnClickRemoveNode,
      SaveGraph
    );
    selectedGraph.AddNode((Node)newNode);
    // selectedGraph.nodes.Add(new ConversationNode(
    //   selectedGraph.GenerateUniqueId(),
    //   mousePosition,
    //   selectedGraph,
    //   OnClickOption,
    //   OnClickNode,
    //   OnClickRemoveNode,
    //   SaveGraph
    // ) as Node);
    SaveGraph(selectedGraph);
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

  protected override void SaveGraph(NodeGraph graph) {
    EditorUtility.SetDirty((Conversation)graph);
    AssetDatabase.SaveAssets();
  }
}
