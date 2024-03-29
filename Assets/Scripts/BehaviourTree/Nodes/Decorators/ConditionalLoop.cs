﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Decorator/ConditionalLoop")]
    public class ConditionalLoop : Decorator
    {
        [Tooltip("接続した Conditional ノードが SUCCESS である間、実行し続けます。"), Output(ShowBackingValue.Never, ConnectionType.Override)] public Node condition;

        protected Node m_conditionNode;

        public override void Setup()
        {
            base.Setup();

            XNode.NodePort outPort = GetOutputPort("condition");
            if (!outPort.IsConnected)
            {
                return;
            }

            m_conditionNode = outPort.Connection.node as Node;
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            if (m_conditionNode.Evaluate(go) == NodeStatus.SUCCESS)
            {
                m_node.Evaluate(go);

                // 子ノードの実行結果によらず、RUNNING ステータスとする
                m_nodeStatus = NodeStatus.RUNNING;
            }
            else
            {
                m_nodeStatus = NodeStatus.FAILURE;
            }

            return m_nodeStatus;
        }

        //TODO: Decorator 基底クラスに移動する
        public override void CollectConditionals(ref List<Conditional> conditionalNodes)
        {
            base.CollectConditionals(ref conditionalNodes);

            conditionalNodes.Add(m_conditionNode as Conditional);
        }
    }
}
