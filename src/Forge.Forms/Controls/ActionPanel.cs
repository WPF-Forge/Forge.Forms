using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Forge.Forms.Controls
{
    public enum Position
    {
        Right,
        Left
    }

    public class ActionPanel : Panel
    {
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached(
                "Position",
                typeof(Position),
                typeof(ActionPanel),
                new FrameworkPropertyMetadata(Position.Right, FrameworkPropertyMetadataOptions.Inherits));

        public static Position GetPosition(DependencyObject element)
        {
            return (Position)element.GetValue(PositionProperty);
        }

        public static void SetPosition(DependencyObject element, Position value)
        {
            element.SetValue(PositionProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize.Height = double.PositiveInfinity;
            var children = InternalChildren;
            var leftWidth = 0d;
            var rightWidth = 0d;
            var leftHeight = 0d;
            var rightHeight = 0d;
            var leftMaxHeight = 0d;
            var rightMaxHeight = 0d;
            var leftMaxWidth = 0d;
            var rightMaxWidth = 0d;

            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child != null)
                {
                    child.Measure(availableSize);
                    var width = child.DesiredSize.Width;
                    var height = child.DesiredSize.Height;
                    if (GetPosition(child) == Position.Left)
                    {
                        leftWidth += width;
                        leftHeight += height;
                        leftMaxWidth = Math.Max(leftMaxWidth, width);
                        leftMaxHeight = Math.Max(leftMaxHeight, height);
                    }
                    else
                    {
                        rightWidth += width;
                        rightHeight += height;
                        rightMaxWidth = Math.Max(rightMaxWidth, width);
                        rightMaxHeight = Math.Max(rightMaxHeight, height);
                    }
                }
            }

            // Test for h h.
            if (leftWidth + rightWidth <= availableSize.Width)
            {
                return new Size(leftWidth + rightWidth, Math.Max(leftMaxHeight, rightMaxHeight));
            }

            if (leftWidth <= availableSize.Width)
            {
                // Test for h / h
                if (rightWidth <= availableSize.Width)
                {
                    return new Size(Math.Max(leftWidth, rightWidth), leftMaxHeight + rightMaxHeight);
                }

                // Return h / v
                return new Size(Math.Max(leftWidth, rightMaxWidth), leftMaxHeight + rightHeight);
            }

            // Test for v / h
            if (rightWidth <= availableSize.Width)
            {
                return new Size(Math.Max(leftMaxWidth, rightWidth), leftHeight + rightMaxHeight);
            }

            // Return v / v
            return new Size(Math.Max(leftMaxWidth, rightMaxWidth), leftHeight + rightHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = InternalChildren;
            var leftWidth = 0d;
            var rightWidth = 0d;
            var leftHeight = 0d;
            var rightHeight = 0d;
            var leftMaxHeight = 0d;
            var rightMaxHeight = 0d;
            var leftMaxWidth = 0d;
            var rightMaxWidth = 0d;
            var leftChildren = new List<UIElement>();
            var rightChildren = new List<UIElement>();

            for (var i = 0; i < children.Count; i++)
            {
                var child = children[i];
                if (child != null)
                {
                    var width = child.DesiredSize.Width;
                    var height = child.DesiredSize.Height;
                    if (GetPosition(child) == Position.Left)
                    {
                        leftWidth += width;
                        leftHeight += height;
                        leftMaxWidth = Math.Max(leftMaxWidth, width);
                        leftMaxHeight = Math.Max(leftMaxHeight, height);
                        leftChildren.Add(child);
                    }
                    else
                    {
                        rightWidth += width;
                        rightHeight += height;
                        rightMaxWidth = Math.Max(rightMaxWidth, width);
                        rightMaxHeight = Math.Max(rightMaxHeight, height);
                        rightChildren.Add(child);
                    }
                }
            }

            // Test for h h.
            if (leftWidth + rightWidth <= finalSize.Width)
            {
                StackHorizontally(leftChildren, 0d, 0d, finalSize.Height);
                StackHorizontally(rightChildren, finalSize.Width - rightWidth, 0d, finalSize.Height);
                return finalSize;
            }

            if (leftWidth <= finalSize.Width)
            {
                // Test for h / h
                if (rightWidth <= finalSize.Width)
                {
                    StackHorizontally(leftChildren, 0d, 0d, leftMaxHeight);
                    StackHorizontally(rightChildren, finalSize.Width - rightWidth, leftMaxHeight, rightMaxHeight);
                    return finalSize;
                }

                // Return h / v
                StackHorizontally(leftChildren, 0d, 0d, leftMaxHeight);
                StackVertically(rightChildren, finalSize.Width - rightMaxWidth, leftMaxHeight, rightMaxWidth);
                return finalSize;
            }

            // Test for v / h
            if (rightWidth <= finalSize.Width)
            {
                StackVertically(leftChildren, 0d, 0d, leftMaxWidth);
                StackHorizontally(rightChildren, finalSize.Width - rightWidth, leftHeight, rightMaxHeight);
                return finalSize;
            }

            // Return v / v
            StackVertically(leftChildren, 0d, 0d, leftMaxWidth);
            StackVertically(rightChildren, finalSize.Width - rightMaxWidth, leftHeight, rightMaxWidth);
            return finalSize;
        }

        private void StackVertically(List<UIElement> children, double x, double y, double width)
        {
            var offset = 0d;
            foreach (var child in children)
            {
                var height = child.DesiredSize.Height;
                child.Arrange(new Rect(x, y + offset, width, height));
                offset += height;
            }
        }

        private void StackHorizontally(List<UIElement> children, double x, double y, double height)
        {
            var offset = 0d;
            foreach (var child in children)
            {
                var width = child.DesiredSize.Width;
                child.Arrange(new Rect(x + offset, y, width, height));
                offset += width;
            }
        }
    }
}
