using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeManager<NODE_GRAPH, NODE, NODE_OPTION, NODE_SIDE_EFFECT> : MonoBehaviour
  where NODE_GRAPH : NodeGraph<NODE, NODE_OPTION, NODE_SIDE_EFFECT>
  where NODE : Node<NODE_OPTION, NODE_SIDE_EFFECT>
  where NODE_OPTION : NodeOption
  where NODE_SIDE_EFFECT : NodeSideEffect {

  protected NODE_GRAPH currentGraph;
  protected NODE currentNode;

  virtual public bool SetNodeGraph(NODE_GRAPH graph) {
    if (currentGraph == null) {
      currentGraph = graph;
      int startNodeID = graph.GetStartNodeId();
      if (startNodeID != -1) {
        SetNode(startNodeID);
        return true;
      } else {
        Debug.LogWarning("Node graph has no start node.");
      }
    }
    return false;
  }

  protected virtual void ClearNodeGraph() {
    currentGraph = null;
  }

  protected virtual void SetNode(int id) {
    currentNode = GetNode(id);
    OnNodeGraphUIUpdate(currentNode);
  }

  protected virtual void SetNode(NODE node) {
    currentNode = node;
    OnNodeGraphUIUpdate(node);
  }

  public void SelectNodeOption(int option) {
    SetNode(currentNode.options[option - 1].next);
  }

  NODE GetNode(int id) {
    foreach (NODE node in currentGraph.nodes) {
      if (node.id == id) {
        return node;
      }
    }
    return null;
  }

  virtual protected void OnNodeGraphUIUpdate(NODE node) { }

}