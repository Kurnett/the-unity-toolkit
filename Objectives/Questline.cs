using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Quests/Questline")]
public class Questline : NodeGraph<QuestNode, QuestOption> { }
