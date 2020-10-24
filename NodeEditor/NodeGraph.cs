using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Nodes/Graph")]
public abstract class NodeGraph : ScriptableObject {
  public int id;
  new public string name;
  public List<Node> nodes = new List<Node>();
  public Node startNode;

  public int GenerateUniqueId() {
    bool idFound = false;
    int i = 0;
    while (!idFound) {
      if (GetNodeById(i) == null) {
        return i;
      }
      i++;
    }
    return -1;
  }

  public int GetStartNodeID() {
    foreach (Node node in nodes) {
      if (node.start) {
        return node.id;
      }
    }
    return -1;
  }

  public Node GetNodeById(int id) {
    if (id != -1) {
      Node node = nodes.Find(nodeInit => nodeInit.id == id);
      if (node != null) {
        return node;
      }
      ClearInvalidOptionIDs(id);
    }
    return null;
  }

  public void ClearInvalidOptionIDs(int id) {
    foreach (Node node in nodes) {
      if (node.defaultOption != null && node.defaultOption.next == id) {
        node.defaultOption.next = -1;
      }
      foreach (NodeOption option in node.options) {
        if (option.next == id) {
          option.next = -1;
        }
      }
    }
  }

  public void SetStartNode(Node startNode) {
    foreach (Node node in nodes) {
      if (startNode == node) {
        node.start = true;
      } else {
        node.start = false;
      }
    }
  }

  public virtual void AddNode(
      Vector2 position,
      Action<NodeOption> OnClickOption,
      Action<Node> OnClickNode,
      Action<Node> OnClickRemoveNode,
      Action<NodeGraph> SaveGraph
    ) {
    Node node = (Node)ScriptableObject.CreateInstance(typeof(Node));
    node.Construct(
      GenerateUniqueId(),
      position,
      OnClickOption,
      OnClickNode,
      OnClickRemoveNode,
      SaveGraph
    );
    AssetDatabase.AddObjectToAsset(node, this);
    nodes.Add(node);
  }

  public void RemoveNode(Node node) {
    foreach (NodeOption option in node.options) {
      AssetDatabase.RemoveObjectFromAsset(option);
    }
    AssetDatabase.RemoveObjectFromAsset(node);
    nodes.Remove(node);
  }
}