using System.Collections.Generic;
using GraphSharp.Algorithms.Highlight;
using QuickGraph;

namespace SeeGit.Models
{
    public class ReachableHighlightAlgorithmFactory<TVertex, TEdge, TGraph> :
        IHighlightAlgorithmFactory<TVertex, TEdge, TGraph> where TVertex : class
                                                           where TEdge : IEdge<TVertex>
                                                           where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
        private const string HighlightModeName = "Reachable";

        public IHighlightAlgorithm<TVertex, TEdge, TGraph> CreateAlgorithm(string highlightMode,
                                                                           IHighlightContext<TVertex, TEdge, TGraph>
                                                                               context,
                                                                           IHighlightController<TVertex, TEdge, TGraph>
                                                                               controller,
                                                                           IHighlightParameters parameters)
        {
            if (string.IsNullOrEmpty(highlightMode) || highlightMode == HighlightModeName)
            {
                return new ReachableHighlightAlgorithm<TVertex, TEdge, TGraph>(controller, parameters);
            }
            return null;
        }

        public IHighlightParameters CreateParameters(string highlightMode, IHighlightParameters oldParameters)
        {
            return new HighlightParameterBase();
        }

        public string GetHighlightMode(IHighlightAlgorithm<TVertex, TEdge, TGraph> algorithm)
        {
            if (algorithm is ReachableHighlightAlgorithm<TVertex, TEdge, TGraph>)
            {
                return HighlightModeName;
            }
            return null;
        }

        public bool IsValidMode(string mode)
        {
            return string.IsNullOrEmpty(mode) || mode == HighlightModeName;
        }

        // Properties
        public IEnumerable<string> HighlightModes
        {
            get { yield return HighlightModeName; }
        }
    }
}