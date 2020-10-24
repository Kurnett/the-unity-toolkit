using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNodeRenderer : NodeRenderer<Questline, QuestNode> {

  public QuestNodeRenderer(Questline graphInit) : base(graphInit) { }

  protected override void DrawHeader(QuestNode node) {
    bool diff = false;
    GUILayout.Box("", GUIStyle.none);
    GUILayout.Label("Quest Log");
    bool startNew = EditorGUILayout.ToggleLeft("Start", node.start);
    string textNew = EditorGUILayout.TextArea(node.text, GUILayout.Height(90));
    if (node.start != startNew) {
      node.start = startNew;
      diff = true;
    }
    if (node.text != textNew) {
      node.text = textNew;
      diff = true;
    }
    if (diff) node.SaveGraph(graph);
  }

  protected override void DrawOptionControlsCenter(QuestNode node, int i) {
    QuestOption option = (QuestOption)node.options[i];
    option.id = EditorGUILayout.TextArea(option.id);
  }

}
