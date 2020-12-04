// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EasyFarm.Views
{
    /// <summary>
    /// Interaction logic for FollowView.xaml
    /// </summary>
    public partial class DebugView : UserControl
    {
        public DebugView()
        {
            InitializeComponent();
        }


        public object ObjectToVisualize
        {
            get { return (object)GetValue(ObjectToVisualizeProperty); }
            set { SetValue(ObjectToVisualizeProperty, value); }
        }
        public static readonly DependencyProperty ObjectToVisualizeProperty =
            DependencyProperty.Register("ObjectToVisualize", typeof(object), typeof(DebugView), new PropertyMetadata(null, OnObjectChanged));

        private static void OnObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeNode tree = TreeNode.CreateTree(e.NewValue);
            (d as DebugView).TreeNodes = new List<TreeNode>() { tree };
        }

        public List<TreeNode> TreeNodes
        {
            get { return (List<TreeNode>)GetValue(TreeNodesProperty); }
            set { SetValue(TreeNodesProperty, value); }
        }
        public static readonly DependencyProperty TreeNodesProperty =
            DependencyProperty.Register("TreeNodes", typeof(List<TreeNode>), typeof(DebugView), new PropertyMetadata(null));
    }
}
