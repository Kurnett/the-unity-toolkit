using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : NodeGraph {
  public override void AddNode(
      Vector2 position,
      Action<NodeOption> OnClickOption,
      Action<Node> OnClickNode,
      Action<Node> OnClickRemoveNode,
      Action<NodeGraph> SaveGraph
    ) {
    ConversationNode node = (ConversationNode)ScriptableObject.CreateInstance(typeof(ConversationNode));
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
}
