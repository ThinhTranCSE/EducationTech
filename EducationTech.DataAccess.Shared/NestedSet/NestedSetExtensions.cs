using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EducationTech.DataAccess.Shared.NestedSet
{
    public static class NestedSetExtensions
    {
        public static IEnumerable<TNestedSetNode> GetChilren<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.EntityNode
                .Where(n => n.Left > currentNode.Left && n.Right < currentNode.Right)
                .OrderBy(n => n.Left);
        }

        public static IEnumerable<TNestedSetNode> GetParents<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.EntityNode
                .Where(n => n.Left < currentNode.Left && n.Right > currentNode.Right)
                .OrderBy(n => n.Left);
        }
        public static TNestedSetNode? GetParent<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return GetParents(nestedSet, currentNode).FirstOrDefault();
        }

        public static IEnumerable<TNestedSetNode> GetTree<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.EntityNode
                .Where(n => n.Left >= currentNode.Left && n.Right <= currentNode.Right)
                .OrderBy(n => n.Left)
                .ToArray();
        }

        public static TNestedSetNode AddNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, int? leftBound, TNestedSetNode addedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            int right = Math.Max(leftBound ?? 0, 0);

            nestedSet.EntityNode
                .Where(n => n.Right > right)
                .ToList()
                .ForEach(n => n.Right += 2);
            
            nestedSet.EntityNode
                .Where(n => n.Left > right)
                .ToList()
                .ForEach(n => n.Left += 2);

            addedNode.Left = right + 1;
            addedNode.Right = right + 2;

            nestedSet.EntityNode.Add(addedNode);

            nestedSet.SaveChanges();

            return addedNode;
        }


        public static void RemoveNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode removedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            int width = removedNode.Right - removedNode.Left + 1;

            var removedNodes = nestedSet.EntityNode
                .Where(n => n.Left >= removedNode.Left && n.Left <= removedNode.Right)
                .ToList();
            nestedSet.EntityNode.RemoveRange(removedNodes);

            nestedSet.EntityNode
                .Where(n => n.Right > removedNode.Right)
                .ToList()
                .ForEach(n => n.Right -= width);

            nestedSet.EntityNode
                .Where(n => n.Left > removedNode.Right)
                .ToList()
                .ForEach(n => n.Left -= width);

            nestedSet.SaveChanges();
        }

        public static IEnumerable<TNestedSetNode> GetImmediateChildren<TNestedSetNode>(this IEnumerable<TNestedSetNode> nodes, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            
            var directChildren = nodes
                .Where(c => c.Left > currentNode.Left && c.Right < currentNode.Right)
                .Select(c => new
                {
                    Child = c,
                    MaxParentLeft = nodes
                        .Where(p => c.Left > p.Left && c.Right < p.Right)
                        .Max(p => (int?)p.Left)
                })
                .Where(x => x.MaxParentLeft == currentNode.Left)
                .Select(x => x.Child)
                .OrderBy(c => c.Left)
                .ToList();

            return directChildren;

        }


        public static ICollection<NestedSetRecursiveNodeDto<TNestedSetNode>> ToTrees<TNestedSetNode>(this IEnumerable<TNestedSetNode> nodes, int left = 0, int? right = null)
            where TNestedSetNode : class, INestedSetNode
        {
            var tree = new Dictionary<TNestedSetNode, ICollection<NestedSetRecursiveNodeDto<TNestedSetNode>>>();
            foreach (var node in nodes)
            {
                if(node.Left == left + 1 && (right == null || node.Right < right))
                {
                    tree.Add(node, nodes.ToTrees(node.Left, node.Right));
                    left = node.Right;
                }
            }
            return tree.Select(kvp => new NestedSetRecursiveNodeDto<TNestedSetNode>
            {
                Node = kvp.Key,
                Children = kvp.Value
            }).OrderBy(n => n.Node.Left).ToList();
        }

    }

     
}
