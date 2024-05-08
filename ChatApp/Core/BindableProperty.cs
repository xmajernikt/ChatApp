using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp.Core
{
    public static class BindableProperty
    {
        public static readonly DependencyProperty VariableProperty =
            DependencyProperty.RegisterAttached(
                "Variable",
                typeof(string),
                typeof(BindableProperty),
                new PropertyMetadata(null));

        public static string GetVariable(DependencyObject obj)
        {
            return (string)obj.GetValue(VariableProperty);
        }

        public static void SetVariable(DependencyObject obj, string value)
        {
            obj.SetValue(VariableProperty, value);
        }

        public static readonly DependencyProperty ImageSrcProperty =
            DependencyProperty.RegisterAttached(
                "ImageSrc",
                typeof(string),
                typeof(BindableProperty),
                new PropertyMetadata(null));

        public static string GetImageSrc(DependencyObject obj)
        {
            return (string)obj.GetValue(ImageSrcProperty);
        }

        public static void SetImageSrc(DependencyObject obj, string value)
        {
            obj.SetValue(ImageSrcProperty, value);
        }
    }
}
