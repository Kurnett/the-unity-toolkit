using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Quests/Questline")]
public class Questline : NodeGraph {
  public override void AddNode(Vector2 position) {
    QuestNode node = (QuestNode)ScriptableObject.CreateInstance(typeof(QuestNode));
    node.Construct(
      GenerateUniqueId(),
      position
    );
    AssetDatabase.AddObjectToAsset(node, this);
    nodes.Add(node);
  }
}
