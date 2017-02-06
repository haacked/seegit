using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace SeeGit
{
    /// <summary>
    /// Custom Adorner hosting a ContentControl with a ContentTemplate
    /// </summary>
    internal class CommitTemplatedAdorner : Adorner
    {
        private readonly FrameworkElement _adornedElement;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="frameworkElementAdorner"></param>
        public CommitTemplatedAdorner(UIElement adornedElement, FrameworkElement frameworkElementAdorner)
            : base(adornedElement)
        {
            _adornedElement = (FrameworkElement) adornedElement;

            // Assure we get mouse hits
            _frameworkElementAdorner = frameworkElementAdorner;
            AddVisualChild(_frameworkElementAdorner);
            AddLogicalChild(_frameworkElementAdorner);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return _frameworkElementAdorner;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override int VisualChildrenCount => 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            //_frameworkElementAdorner.Width = constraint.Width;
            //_frameworkElementAdorner.Height = constraint.Height;
            _frameworkElementAdorner.Measure(constraint);

            return _frameworkElementAdorner.DesiredSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Make sure to align to the right of the element being adorned
            double xloc = _adornedElement.ActualWidth;
            _frameworkElementAdorner.Arrange(new Rect(new Point(xloc, 0), finalSize));
            return finalSize;
        }


        /// <summary>
        /// 
        /// </summary>
        private readonly FrameworkElement _frameworkElementAdorner;
    }
}