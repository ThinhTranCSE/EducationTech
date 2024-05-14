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
        public static IEnumerable<INestedSetNode> GetChilren<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.Model
                .Where(n => n.TreeId == currentNode.TreeId && n.Left > currentNode.Left && n.Right < currentNode.Right)
                .OrderBy(n => n.Left);
        }

        public static IEnumerable<INestedSetNode> GetParents<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.Model
                .Where(n => n.TreeId == currentNode.TreeId && n.Left < currentNode.Left && n.Right > currentNode.Right)
                .OrderBy(n => n.Left);
        }
        public static INestedSetNode? GetParent<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return GetParents(nestedSet, currentNode).FirstOrDefault();
        }

        public static IEnumerable<INestedSetNode> GetTree<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode currentNode)
            where TNestedSetNode : class, INestedSetNode
        {
            return nestedSet.Model
                .Where(n => n.TreeId == currentNode.TreeId && n.Left >= currentNode.Left && n.Right <= currentNode.Right)
                .OrderBy(n => n.Left);
        }

        public static INestedSetNode AddNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, INestedSetNode? parentNode, TNestedSetNode addedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            if(parentNode == null)
            {
                int newTreeId = nestedSet.Model.Max(n => n.TreeId) + 1;
                addedNode.TreeId = newTreeId;
                addedNode.Left = 1;
                addedNode.Right = 2;
                nestedSet.Model.Add(addedNode);
                nestedSet.SaveChanges();
                return addedNode;
            }

            nestedSet.Model
                .Where(n => n.TreeId == parentNode.TreeId && n.Left > parentNode.Right)
                .ToList()
                .ForEach(n => n.Left += 2);

            nestedSet.Model
                .Where(n => n.TreeId == parentNode.TreeId && n.Right >= parentNode.Right)
                .ToList()
                .ForEach(n => n.Right += 2);

            addedNode.TreeId = parentNode.TreeId;
            addedNode.Left = parentNode.Right;
            addedNode.Right = parentNode.Right + 1;
            nestedSet.Model.Add(addedNode);

            nestedSet.SaveChanges();
            return addedNode;
        }

        public static void RemoveNode<TNestedSetNode>(this INestedSet<TNestedSetNode> nestedSet, TNestedSetNode removedNode)
            where TNestedSetNode : class, INestedSetNode
        {
            int width = removedNode.Right - removedNode.Left + 1;

            nestedSet.Model
                .Where(n => n.TreeId == removedNode.TreeId && n.Left > removedNode.Right)
                .ToList()
                .ForEach(n => n.Left -= width);

            nestedSet.Model
                .Where(n => n.TreeId == removedNode.TreeId && n.Right > removedNode.Right)
                .ToList()
                .ForEach(n => n.Right -= width);

            nestedSet.Model.Remove(removedNode);
            nestedSet.SaveChanges();
        }

        public static IEnumerable<INestedSetNode> GetImmediateChildren(this IEnumerable<INestedSetNode> nodes, INestedSetNode currentNode)
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

        public static NestedSetRecursiveDto ToRecursiveViewing(this IEnumerable<INestedSetNode> nodes, INestedSetNode? root = null)
        {
            if(nodes.Count() == 0)
            {
                return new NestedSetRecursiveDto();
            }
            if (root == null)
            {
                root = nodes.OrderBy(n => n.Left).FirstOrDefault()!;
            }
            return new NestedSetRecursiveDto
            {
                Node = root,
                Children = nodes.GetImmediateChildren(root).Select(c => nodes.ToRecursiveViewing(c))
            };
        }
    }
}
