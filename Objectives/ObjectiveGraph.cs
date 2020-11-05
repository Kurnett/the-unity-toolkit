using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Objectives/ObjectiveGraph")]
public class ObjectiveGraph : NodeGraph<ObjectiveNode, ObjectiveOption> { }
