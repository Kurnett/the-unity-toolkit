using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<NODE_GRAPH, NODE, NODE_OPTION>
  where NODE_GRAPH : NodeGraph<NODE, NODE_OPTION>
  where NODE : Node<NODE_OPTION>
  where NODE_OPTION : NodeOption {

  public virtual void DrawNodeGraph(NODE_GRAPH graph) {
    if (graph.nodes != null) {
      foreach (NODE node in graph.nodes) {
        NodeRenderer<NODE_GRAPH, NODE, NODE_OPTION> r = new NodeRenderer<NODE_GRAPH, NODE, NODE_OPTION>(graph);
        r.DrawNode(node);
      }
    }
  }
}