using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestlineRenderer : NodeGraphRenderer<Questline, QuestNode> {

  public override void DrawNodeGraph(Questline graph) {
    if (graph.nodes != null) {
      foreach (QuestNode node in graph.nodes) {
        QuestNodeRenderer r = new QuestNodeRenderer(graph);
        r.DrawNode(node);
      }
    }
  }
}