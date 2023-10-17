using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BTGraphNode : Node
{
    public BTNode Node { get; private set; }

    Port inputPort;
    Port outputPort;

    public Port GetInputPort() { return inputPort; }
    public Port GetOutputPort() { return outputPort; }  

    public BTGraphNode(BTNode node)
    {
        Node = node;
        title = node.GetType().Name;
        style.left = node.GetGraphPosition().x;
        style.top = node.GetGraphPosition().y;

        CreatePorts();
    }

    private void CreatePorts()
    {
        switch(Node.GetInputPortType())
        {
            case BTNodePortType.Single:
                inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(BTNode));
                inputPort.portName = "parent";
                break;

            default:
                break;
        }

        if(inputPort != null)
            inputContainer.Add(inputPort);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Node.SetGraphPosition(new Vector2(newPos.xMin, newPos.yMin));
    }
}
