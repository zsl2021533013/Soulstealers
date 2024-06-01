using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
    public static class GraphExtension
    {
        private const float NodeDeltaX = 50f;
        private const float NodeDeltaY = 120f;
        
        public static void AlignTree(this Graph graph)
        {
            var tree = BuildTree(graph.primeNode);
            
            FirstAlignTraverse(graph.primeNode, tree);
            
            SecondAlignTraverse(graph.primeNode, tree);
        }
        
        private static Dictionary<Node, List<Node>> BuildTree(Node root)
        {
            var queue = new Queue<Node>();
            var flag = new Dictionary<Node, bool>();
            var ans = new Dictionary<Node, List<Node>>();
            
            queue.Enqueue(root);
            flag.Add(root, true);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                
                var children = node.GetChildNodes()
                    .Where(child =>
                    {
                        if (!flag.ContainsKey(child))
                        {
                            flag[child] = false;
                        }
                        
                        return !flag[child];
                    })
                    .ToList();
                ans[node] = children;
                
                children.ForEach(child =>
                {
                    flag[child] = true;
                    queue.Enqueue(child);
                });
            }

            return ans;
        }

        // 第一次遍历，奠定基础框架
        private static void FirstAlignTraverse(Node node, Dictionary<Node, List<Node>> tree, float depth = 0)
        {
            node.position = new Vector2(node.position.x, depth);
            
            var children = node.GetChildren(tree);
            var leftBro = node.GetLeftBrother(tree);
            
            if (children.Count <= 0)
            {
                if (leftBro != null)
                {
                    var leftBroRect = leftBro.rect;
                    
                    var rect = node.rect;
                    rect.position = leftBroRect.position + new Vector2(leftBroRect.size.x + NodeDeltaX, 0f);
                    
                    node.position = rect.position;
                }
                else
                {
                    var rect = node.rect;
                    rect.center = new Vector2(0f, rect.center.y);
                    
                    node.position = rect.position;
                }
            }
            else
            {
                foreach (var child in children)
                {
                    FirstAlignTraverse(child, tree, depth + node.rect.size.y + NodeDeltaY);
                }
                
                if (leftBro != null)
                {
                    var midpoint = 
                        (children[0].rect.center.x + children[^1].rect.center.x) / 2;
                
                    var leftBroRect = leftBro.rect;
                    
                    var rect = node.rect;
                    
                    rect.center = new Vector2(midpoint, leftBroRect.center.y);
                    
                    node.position = rect.position;
                    
                    var targetPos = leftBroRect.position.x + leftBroRect.size.x + NodeDeltaX;
                    
                    var delta = targetPos + rect.size.x / 2 - midpoint;
                    
                    node.Traverse(node =>
                    {
                        var rect = node.rect;
                        rect.position += new Vector2(delta, 0);
                        node.position = rect.position;
                    }, tree);
                }
                else
                {
                    var midpoint = 
                        (children[0].rect.center.x + children[^1].rect.center.x) / 2;
                
                    var rect = node.rect;
                    rect.center = new Vector2(midpoint, rect.center.y);
                    
                    node.position = rect.position;
                }
            }
        }
        
        // 第二次遍历，微调结构，避免线的交叉
        private static void SecondAlignTraverse(Node node, Dictionary<Node, List<Node>> tree)
        {
            var children = node.GetChildren(tree);
            var leftBro = node.GetLeftBrother(tree);

            foreach (var child in children)
            {
                SecondAlignTraverse(child, tree);
            }
            
            for (var i = 0; i < children.Count; i++)
            {
                var rightMostNode = children[i].GetRightMostNode(tree);
                for (var j = i + 1; j < children.Count; j++)
                {
                    var leftMostNode = children[j].GetLeftMostNode(tree);
                    
                    if (rightMostNode.rect.xMax > leftMostNode.rect.xMin)
                    {
                        var delta = rightMostNode.rect.xMax + NodeDeltaX - leftMostNode.rect.xMin;
                        for (var k = j; k < children.Count; k++)
                        {
                            var child = children[k];
                            child.Traverse(node =>
                            {
                                var rect = node.rect;
                                rect.position += new Vector2(delta, 0);
                                node.position = rect.position;
                            }, tree);
                        }
                    }
                }

                #region Adjust Self

                var rect = node.rect;
                rect.center = new Vector2((children[0].rect.center.x + children[^1].rect.center.x) / 2, node.rect.center.y);
                node.position = rect.position;
                
                #endregion
            }
        }
    }
}