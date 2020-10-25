using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Nodes/Graph")]
public abstract class NodeGraph<NODE, NODE_OPTION> : ScriptableObject where NODE : Node<NODE_OPTION> where NODE_OPTION : NodeOption {
  public int id;
  new public string name;
  public List<NODE> nodes = new List<NODE>();
  public NODE startNode;

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
    foreach (NODE node in nodes) {
      if (node.start) {
        return node.id;
      }
    }
    return -1;
  }

  public NODE GetNodeById(int id) {
    if (id != -1) {
      NODE node = nodes.Find(nodeInit => nodeInit.id == id);
      if (node != null) {
        return node;
      }
      ClearInvalidOptionIDs(id);
    }
    return null;
  }

  public void ClearInvalidOptionIDs(int id) {
    foreach (NODE node in nodes) {
      if (node.defaultOption != null && node.defaultOption.next == id) {
        node.defaultOption.next = -1;
      }
      foreach (NODE_OPTION option in node.options) {
        if (option.next == id) {
          option.next = -1;
        }
      }
    }
  }

  public void SetStartNode(NODE startNode) {
    foreach (NODE node in nodes) {
      if (startNode == node) {
        node.start = true;
      } else {
        node.start = false;
      }
    }
  }

  public virtual void AddNode(Vector2 position) {
    NODE node = (NODE)ScriptableObject.CreateInstance(typeof(NODE));
    node.Construct(
      GenerateUniqueId(),
      position
    );
    AssetDatabase.AddObjectToAsset(node, this);
    nodes.Add(node);
  }

  public void RemoveNode(NODE node) {
    foreach (NODE_OPTION option in node.options) {
      AssetDatabase.RemoveObjectFromAsset(option);
    }
    AssetDatabase.RemoveObjectFromAsset(node);
    nodes.Remove(node);
  }
}