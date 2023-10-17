using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BTNodeGraph : GraphView
{
    public new class UxmlFactory : UxmlFactory<BTNodeGraph, UxmlTraits> { }

    BehaviorTree tree;

    public BTNodeGraph()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
        graphViewChanged += GraphChange;
    }

    //this function will be called if the graph changes.
    private GraphViewChange GraphChange(GraphViewChange graphViewChange)
    {
        if(graphViewChange.edgesToCreate != null)
        {
            HandleEdgeCreation(graphViewChange.edgesToCreate);
        }

        if(graphViewChange.elementsToRemove != null)
        {
            HadleElementRemoval(graphViewChange.elementsToRemove);
        }

        return graphViewChange;
    }

    private void HadleElementRemoval(List<GraphElement> elementsToRemove)
    {
        GraphElement elementToKeep = null;

        foreach(GraphElement element in elementsToRemove)
        {
            BTGraphNode graphNode = element as BTGraphNode;
            if(graphNode.Node.GetType() == typeof(BTNode_Root))
            {
                elementToKeep = element;
            }
        }

        if(elementToKeep != null)
        {
            elementsToRemove.Remove(elementToKeep);
        }
    }

    private void HandleEdgeCreation(List<Edge> edgesToCreate)
    {
        foreach(Edge edge in edgesToCreate)
        {
            BTGraphNode inputGraphNode = edge.output.node as BTGraphNode;
            BTGraphNode outputGraphNode = edge.input.node as BTGraphNode;
        
            IBTNodeParent parent = inputGraphNode.Node as IBTNodeParent;
            if(parent != null)
            {
                parent.AddChild(outputGraphNode.Node);
            }
        }
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        var allNodeTypes = TypeCache.GetTypesDerivedFrom<BTNode>();
        foreach ( System.Type type in allNodeTypes )
        {
            if (type.IsAbstract) continue;
            if (type == typeof(BTNode_Root)) continue;

            evt.menu.AppendAction(type.Name, (arg) => CreateNode(type));
        }

    }

    private void CreateNode(Type nodeType)
    {
        BTNode newNode = tree.CreateNode(nodeType);
        CreateGraphNode(newNode);
    }

    private void CreateGraphNode(BTNode newNode)
    {
        BTGraphNode newGraphNode = new BTGraphNode(newNode);
        AddElement(newGraphNode);
    }

    internal void PoulateTree(BehaviorTree selectedAsTree)
    {
        SaveTree();
        
        graphViewChanged -= GraphChange;
        DeleteElements(graphElements);
        graphViewChanged += GraphChange;

        tree = selectedAsTree;
        tree.PreConstruct(); //ensures that there is the root node
        foreach(BTNode node in tree.GetNodes())
        {
            CreateGraphNode(node);
        }
    }

    internal void SaveTree()
    {
        if(tree)
        {
            tree.SaveTree();
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        foreach (Port port in ports)
        {
            if (startPort == port) continue;
            if (port.node == startPort.node) continue;
            if(port.direction == startPort.direction) continue;
            
            BTGraphNode graphNode = port.node as BTGraphNode;
            BTGraphNode startPortGraphNode = startPort.node as BTGraphNode;

            BTNode node = graphNode.Node;
            BTNode startNode = startPortGraphNode.Node;

            if(startPort.direction == Direction.Output)
            {
                if(node.Contains(startNode))
                {
                    continue;
                }
            }

            compatiblePorts.Add(port);
        }

        return compatiblePorts;
    }
}
