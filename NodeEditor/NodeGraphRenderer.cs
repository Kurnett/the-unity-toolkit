using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<NODE_GRAPH, NODE, NODE_OPTION, NODE_RENDERER, NODE_SIDE_EFFECT>
  where NODE_GRAPH : NodeGraph<NODE, NODE_OPTION, NODE_SIDE_EFFECT>
  where NODE : Node<NODE_OPTION, NODE_SIDE_EFFECT>
  where NODE_OPTION : NodeOption
  where NODE_RENDERER : NodeRenderer<NODE_GRAPH, NODE, NODE_OPTION, NODE_SIDE_EFFECT>, new()
  where NODE_SIDE_EFFECT : NodeSideEffect {

  public virtual void DrawNodeGraph(
    NODE_GRAPH graph,
    Action<NODE> OnRemoveNode,
    Action<NODE_OPTION> OnRemoveConnection,
    Action<NODE_OPTION> OnClickOption,
    Action<NODE_GRAPH> SaveGraph
  ) {
    if (graph.nodes != null) {
      foreach (NODE node in graph.nodes) {
        NODE_RENDERER r = new NODE_RENDERER();
        r.Initialize(graph, OnRemoveNode, OnRemoveConnection, OnClickOption, SaveGraph);
        r.DrawNode(node);
      }
    }
  }
}