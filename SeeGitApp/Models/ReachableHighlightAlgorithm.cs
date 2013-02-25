using GraphSharp.Algorithms.Highlight;
using QuickGraph;

namespace SeeGit.Models
{
    public class ReachableHighlightAlgorithm<TVertex, TEdge, TGraph> :
        HighlightAlgorithmBase<TVertex, TEdge, TGraph, IHighlightParameters> where TVertex : class
                                                                             where TEdge : IEdge<TVertex>
                                                                             where TGraph : class,
                                                                                 IBidirectionalGraph<TVertex, TEdge>
    {
        // Methods
        public ReachableHighlightAlgorithm(IHighlightController<TVertex, TEdge, TGraph> controller,
                                           IHighlightParameters parameters) : base(controller, parameters)
        {
        }

        private void ClearAllHighlights()
        {
            ClearSemiHighlights();
            foreach (TVertex local in Controller.HighlightedVertices)
            {
                Controller.RemoveHighlightFromVertex(local);
            }
            foreach (TEdge local2 in Controller.HighlightedEdges)
            {
                Controller.RemoveHighlightFromEdge(local2);
            }
        }

        private void ClearSemiHighlights()
        {
            foreach (var vertex in Controller.SemiHighlightedVertices)
            {
                Controller.RemoveSemiHighlightFromVertex(vertex);
            }
            foreach (var edge in Controller.SemiHighlightedEdges)
            {
                Controller.RemoveSemiHighlightFromEdge(edge);
            }
        }

        public override bool OnEdgeHighlighting(TEdge edge)
        {
            this.ClearAllHighlights();
            if (!(!object.Equals(edge, default(TEdge)) && base.Controller.Graph.ContainsEdge(edge)))
            {
                return false;
            }
            Controller.HighlightEdge(edge, null);
            Controller.SemiHighlightVertex(edge.Source, "Source");
            Controller.SemiHighlightVertex(edge.Target, "Target");
            return true;
        }

        public override bool OnEdgeHighlightRemoving(TEdge edge)
        {
            ClearAllHighlights();
            return true;
        }

        public override bool OnVertexHighlighting(TVertex vertex)
        {
            if (!Controller.Graph.IsDirected) return false;
            ClearAllHighlights();

            if (vertex == null || !Controller.Graph.ContainsVertex(vertex)) return false;

            Controller.HighlightVertex(vertex, "Source");
            return HighlightChildren(vertex);
        }

        private bool HighlightChildren(TVertex vertex)
        {
            if (vertex == null) return false;

            foreach (var outEdge in Controller.Graph.OutEdges(vertex))
            {
                Controller.SemiHighlightEdge(outEdge, "OutEdge");
                if (Controller.IsSemiHighlightedVertex(outEdge.Target)) continue;
                Controller.SemiHighlightVertex(outEdge.Target, "Target");
                HighlightChildren(outEdge.Target);
            }
            return true;
        }

        public override bool OnVertexHighlightRemoving(TVertex vertex)
        {
            ClearAllHighlights();
            return true;
        }

        public override void ResetHighlight()
        {
            ClearAllHighlights();
        }
    }
}