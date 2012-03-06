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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornedElement"></param>
        public CommitTemplatedAdorner(UIElement adornedElement, FrameworkElement frameworkElementAdorner)
            : base(adornedElement)
        {
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
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

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
            _frameworkElementAdorner.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }


        /// <summary>
        /// 
        /// </summary>
        private readonly FrameworkElement _frameworkElementAdorner;
    }
}