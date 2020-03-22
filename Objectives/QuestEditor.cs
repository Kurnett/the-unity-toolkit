using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestEditor : NodeEditor<Questline> {

  [MenuItem("Window/Quest Editor")]
  private static void OpenWindow() {
    QuestEditor window = GetWindow<QuestEditor>();
    window.titleContent = new GUIContent("Quest Editor");
  }

  protected override void RenderNoConversationSelectedGUI() {
    centerText = new GUIStyle();
    centerText.alignment = TextAnchor.MiddleCenter;
    EditorGUI.LabelField(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 25, 400, 50), "Select a questline to get started", centerText);
  }

  // TODO: Move node instantiation to NodeGraph class.

  protected override void OnClickAddNode(Vector2 mousePosition) {
    if (selectedGraph != null && selectedGraph.nodes == null) { selectedGraph.nodes = new List<Node>(); }
    QuestNode newNode = (QuestNode)ScriptableObject.CreateInstance(typeof(QuestNode));
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
    EditorUtility.SetDirty((Questline)graph);
    AssetDatabase.SaveAssets();
  }
}
