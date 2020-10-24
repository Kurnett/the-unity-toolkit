using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : NodeGraph<ConversationNode, ConversationOption> {
  public override void AddNode(Vector2 position) {
    ConversationNode node = (ConversationNode)ScriptableObject.CreateInstance(typeof(ConversationNode));
    node.Construct(
      GenerateUniqueId(),
      position
    );
    AssetDatabase.AddObjectToAsset(node, this);
    nodes.Add(node);
  }
}
