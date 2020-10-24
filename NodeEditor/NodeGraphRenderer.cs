using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<T, J> where T : NodeGraph where J : Node {

  // protected K editor;

  // public NodeGraphRenderer(K editorInit) {
  //   editor = editorInit;
  // }

  public virtual void DrawNodeGraph(T graph) {
    if (graph.nodes != null) {
      foreach (Node node in graph.nodes) {
        NodeRenderer<T, J> r = new NodeRenderer<T, J>(graph);
        r.DrawNode((J)node);
      }
    }
  }
}