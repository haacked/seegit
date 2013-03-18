using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            ((CommitVertex)Model).Expanded = true;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ((CommitVertex)Model).Expanded = false;
        }
    }
}
