using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestNodeRenderer : NodeRenderer<QuestNode> {

  public QuestNodeRenderer(QuestNode nodeInit) : base(nodeInit) { }

  protected override void DrawHeader() {
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
    if (diff) node.SaveGraph(node.graph);
  }

  protected override void DrawOptionControlsCenter(int i) {
    QuestOption option = (QuestOption)node.options[i];
    option.id = EditorGUILayout.TextArea(option.id);
  }

}
