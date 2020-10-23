using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<T, J> where T : NodeGraph where J : Node {
  public virtual void DrawNodeGraph(T graph) {
    if (graph.nodes != null) {
      foreach (Node node in graph.nodes) {
        NodeRenderer<J> r = new NodeRenderer<J>((J)node);
        r.DrawNode(node);
      }
    }
  }
}