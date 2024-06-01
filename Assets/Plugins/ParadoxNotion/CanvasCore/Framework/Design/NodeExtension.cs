using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;

namespace NodeCanvas.DialogueTrees
{
    public static class NodeExtension
    {
        public static List<Node> GetChildren(this Node node, Dictionary<Node, List<Node>> tree)
        {
            return tree[node];
        }
        
        public static Node GetParent(this Node node, Dictionary<Node, List<Node>> tree)
        {
            var pair = tree.ToList().FirstOrDefault(pair =>
            {
                var key = pair.Key;
                var value = pair.Value;

                return value.Contains(node);
            });
            
            return pair.Key;
        }
        
        public static List<Node> GetBrother(this Node node, Dictionary<Node, List<Node>> tree)
        {
            var parent = node.GetParent(tree);
            
            return parent.GetChildren(tree).Where(child => child != node).ToList();
        }
        
        public static Node GetLeftBrother(this Node node, Dictionary<Node, List<Node>> tree)
        {
            if (node.GetParent(tree) == null)
            {
                return null;
            }
            
            var parent = node.GetParent(tree);
            var children = parent.GetChildren(tree);

            Node leftBro = null;
            foreach (var child in children)
            {
                if (child == node)
                {
                    return leftBro;
                }
                
                leftBro = child;
            }
            
            return leftBro;
        }
        
        public static Node GetLeftMostNode(this Node node, Dictionary<Node, List<Node>> tree)
        {
            var leftMost = node.rect.xMin;
            var leftMostNode = node;
            
            node.Traverse(mostNode =>
            {
                if (mostNode.rect.xMin < leftMost)
                {
                    leftMost = mostNode.rect.xMin;
                    leftMostNode = mostNode;
                }
            }, tree);
            
            return leftMostNode;
        }
        
        public static Node GetRightMostNode(this Node node, Dictionary<Node, List<Node>> tree)
        {
            var rightMost = node.rect.xMax;
            var rightMostNode = node;
            
            node.Traverse(mostNode =>
            {
                if (mostNode.rect.xMax > rightMost)
                {
                    rightMost = mostNode.position.x;
                    rightMostNode = mostNode;
                }
            }, tree);
            
            return rightMostNode;
        }
        
        public static List<Node> GetLeftBrothers(this Node node, Dictionary<Node, List<Node>> tree)
        {
            var leftBros = new List<Node>();
            
            if (node.GetParent(tree) == null)
            {
                return leftBros;
            }
            
            var parent = node.GetParent(tree);
            var children = parent.GetChildren(tree);
            
            foreach (var child in children)
            {
                if (child == node)
                {
                    return leftBros;
                }
                
                leftBros.Add(child);
            }
            
            return leftBros;
        }
        
        public static void Traverse(this Node node, Action<Node> visitor, Dictionary<Node, List<Node>> tree) {
            if (node != null) 
            {
                visitor.Invoke(node);
                var children = node.GetChildren(tree);
                children.ForEach((n) => Traverse(n, visitor, tree));
            }
        }
    }
}