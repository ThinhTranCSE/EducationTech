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
                .Where(n => n.TreeId == currentNode.TreeId && n.Left > currentNode.Left && n.Right < currentNode.Right)
                .OrderBy(n => n.Left);
        }

        public static IEnumerable<TNestedSetNode> GetParents<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.EntityNode
                .Where(n => n.TreeId == currentNode.TreeId && n.Left < currentNode.Left && n.Right > currentNode.Right)
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
                .Where(n => n.TreeId == currentNode.TreeId && n.Left >= currentNode.Left && n.Right <= currentNode.Right)
                .OrderBy(n => n.Left);
        }

        public static TNestedSetNode AddNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode? parentNode, TNestedSetNode addedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            if(parentNode == null)
            {   
                int newTreeId = nestedSet.EntityNode.Count() == 0 ? 1 : nestedSet.EntityNode.Max(n => n.TreeId) + 1;
                addedNode.TreeId = newTreeId;
                addedNode.Left = 1;
                addedNode.Right = 2;
                nestedSet.EntityNode.Add(addedNode);
                nestedSet.SaveChanges();
                return addedNode;
            }

            nestedSet.EntityNode
                .Where(n => n.TreeId == parentNode.TreeId && n.Left > parentNode.Right)
                .ToList()
                .ForEach(n => n.Left += 2);

            nestedSet.EntityNode
                .Where(n => n.TreeId == parentNode.TreeId && n.Right >= parentNode.Right)
                .ToList()
                .ForEach(n => n.Right += 2);

            addedNode.TreeId = parentNode.TreeId;
            addedNode.Left = parentNode.Right;
            addedNode.Right = parentNode.Right + 1;
            nestedSet.EntityNode.Add(addedNode);

            nestedSet.SaveChanges();
            return addedNode;
        }


        public static void RemoveNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode removedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            int width = removedNode.Right - removedNode.Left + 1;

            nestedSet.EntityNode
                .Where(n => n.TreeId == removedNode.TreeId && n.Left > removedNode.Right)
                .ToList()
                .ForEach(n => n.Left -= width);

            nestedSet.EntityNode
                .Where(n => n.TreeId == removedNode.TreeId && n.Right > removedNode.Right)
                .ToList()
                .ForEach(n => n.Right -= width);

            nestedSet.EntityNode.Remove(removedNode);
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

        //public static NestedSetRecursiveDto<TNestedSetNode> ToRecursiveViewing<TNestedSetNode>(this IEnumerable<TNestedSetNode> nodes, TNestedSetNode? root = null)
        //    where TNestedSetNode : class, INestedSetNode
        //{
        //    if(nodes.Count() == 0)
        //    {
        //        return new NestedSetRecursiveDto<TNestedSetNode>();
        //    }
        //    if (root == null)
        //    {
        //        root = nodes.OrderBy(n => n.Left).FirstOrDefault()!;
        //    }
        //    return new NestedSetRecursiveDto<TNestedSetNode>
        //    {
        //        Node = root,
        //        Children = nodes.GetImmediateChildren(root).Select(c => nodes.ToRecursiveViewing(c))
        //    };
        //}

        public static IEnumerable<NestedSetRecursiveNodeDto<TNestedSetNode>> ToTrees<TNestedSetNode>(this IEnumerable<TNestedSetNode> nodes)
            where TNestedSetNode : class, INestedSetNode
        {
            var nodeDictionay = new Dictionary<int, NestedSetRecursiveNodeDto<TNestedSetNode>>();

            foreach(var node in nodes)
            {
                var recursiveNode = new NestedSetRecursiveNodeDto<TNestedSetNode>
                {
                    Node = node
                };
                nodeDictionay.TryAdd(node.Id, recursiveNode);
            }
            foreach(var node in nodes)
            {
                if(node.ParentId == null)
                {
                    continue;
                }
                if(nodeDictionay.TryGetValue(node.ParentId.Value, out var parentRecursiveNode))
                {
                    var recursiveNode = nodeDictionay[node.Id];
                    parentRecursiveNode.Children.Add(recursiveNode);
                }
            }
            return nodeDictionay.Values.Where(n => n.Node.ParentId == null);

        }

    }

     
}
