using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SLWizard.Resources
{
    public class CubeButton:Button
    {
        //public Geometry PathData
        //{
        //    get { return (Geometry)GetValue(PathDataProperty); }
        //    set { SetValue(PathDataProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ImgSource.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PathDataProperty =
        //    DependencyProperty.Register("PathData", typeof(Geometry), typeof(CubeButton), new PropertyMetadata(null));

        public Brush PathFill
        {
            get { return (Brush)GetValue(PathFillProperty); }
            set { SetValue(PathFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathFillProperty =
            DependencyProperty.Register("PathFill", typeof(Brush), typeof(CubeButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));



        public Brush PathEdge
        {
            get { return (Brush)GetValue(PathEdgeProperty); }
            set { SetValue(PathEdgeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathEdge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathEdgeProperty =
            DependencyProperty.Register("PathEdge", typeof(Brush), typeof(CubeButton), new PropertyMetadata(new SolidColorBrush(Colors.White)));



        public PathFigureCollection PathFigure
        {
            get { return (PathFigureCollection)GetValue(PathFigureProperty); }
            set { SetValue(PathFigureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PathFigure.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathFigureProperty =
            DependencyProperty.Register("PathFigure", typeof(PathFigureCollection), typeof(CubeButton), new PropertyMetadata(null));


    }
}
