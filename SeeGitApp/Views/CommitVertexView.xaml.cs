using System.Windows;
using System.Windows.Controls;

namespace SeeGit.Views
{
    /// <summary>
    /// Interaction logic for CommitVertexView.xaml
    /// </summary>
    public partial class CommitVertexView : UserControl
    {
        public CommitVertexView()
        {
            InitializeComponent();
        }

        public CommitVertex Model
        {
            get { return (CommitVertex)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(CommitVertex), typeof(CommitVertexView), new UIPropertyMetadata(null));

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Model.Expanded = true;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Model.Expanded = false;
        }
    }
}
