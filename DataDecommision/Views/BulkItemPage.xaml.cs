﻿
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataDecommision
{
    /// <summary>
    /// Interaction logic for BulkItemPage.xaml
    /// </summary>
    public partial class BulkItemPage : Page
    {
       
        private static BulkItemPage _instance;

        private BulkItemPage()
        {

            InitializeComponent();
            this.DataContext = new BulkItemVM();
        }

        public static BulkItemPage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BulkItemPage();
                }
                return _instance;
            }
        }
    
    }


    public static class LabelBlinkBehavior
    {
        public static bool GetIsBlinking(Label label)
        {
            return (bool)label.GetValue(IsBlinkingProperty);
        }

        public static void SetIsBlinking(Label label, bool value)
        {
            label.SetValue(IsBlinkingProperty, value);
        }

        public static readonly DependencyProperty IsBlinkingProperty =
            DependencyProperty.RegisterAttached(
                "IsBlinking", typeof(bool), typeof(LabelBlinkBehavior),
                new UIPropertyMetadata(false, OnIsBlinkingChanged));

        private static void OnIsBlinkingChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Label label))
                return;

            bool isBlinking = (bool)e.NewValue;
            if (isBlinking)
            {
                Storyboard storyboard = new Storyboard();
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                Storyboard.SetTarget(animation, label);
                Storyboard.SetTargetProperty(animation,
                    new PropertyPath(UIElement.OpacityProperty));
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }


    }

}
