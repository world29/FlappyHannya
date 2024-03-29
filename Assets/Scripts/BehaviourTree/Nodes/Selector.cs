﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Selector")]
    public class Selector : Composite
    {
        public override void Setup()
        {
            base.Setup();

            SetupNodesOrderByPriority();
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            // Conditional ノードの再評価
            ReevaluateConditionals(go);

            for (int i = m_nodeIndex; i < m_nodes.Count; i++)
            {
                m_nodeStatus = m_nodes[i].Evaluate(go);

                if (m_nodeStatus == NodeStatus.FAILURE)
                {
                    // 次回の評価は先頭ノードから開始する
                    //m_nodeIndex = 0;

                    // 次のノードを評価する
                    continue;
                }
                else if (m_nodeStatus == NodeStatus.RUNNING)
                {
                    // 次回の評価はこのノードから開始する
                    m_nodeIndex = i;

                    // 次のノードを評価しない
                    break;
                }
                else if (m_nodeStatus == NodeStatus.SUCCESS)
                {
                    // 次回の評価は先頭ノードから開始する
                    //m_nodeIndex = 0;

                    // 次のノードを評価しない
                    break;
                }
                else
                {
                    Debug.AssertFormat(false, "invalid node status: {0}", m_nodeStatus);
                    break;
                }
            }

            return m_nodeStatus;
        }

        public override void OnCreateConnection(XNode.NodePort from, XNode.NodePort to)
        {
            base.OnCreateConnection(from, to);

            // children detached
            if (Outputs.Contains(from) || Outputs.Contains(to))
            {
                SetupNodesOrderByPriority();
            }
        }

        public override void OnRemoveConnection(XNode.NodePort port)
        {
            base.OnRemoveConnection(port);

            // children detached
            if (Outputs.Contains(port))
            {
                SetupNodesOrderByPriority();
            }
        }
    }
}
