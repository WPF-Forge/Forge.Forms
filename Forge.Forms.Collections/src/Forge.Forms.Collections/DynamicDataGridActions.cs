using System;
using System.Windows;
using System.Windows.Input;

namespace Forge.Forms.Collections
{
    public partial class DynamicDataGrid
    {
        public static readonly DependencyProperty DeleteActionTextProperty = DependencyProperty.Register(
            nameof(DeleteActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Delete"));

        public string DeleteActionText
        {
            get { return (string) GetValue(DeleteActionTextProperty); }
            set { SetValue(DeleteActionTextProperty, value); }
        }

        public static readonly DependencyProperty CreateActionTextProperty = DependencyProperty.Register(
            nameof(CreateActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Add"));

        public string CreateActionText
        {
            get { return (string) GetValue(CreateActionTextProperty); }
            set { SetValue(CreateActionTextProperty, value); }
        }

        public static readonly DependencyProperty EditActionTextProperty = DependencyProperty.Register(
            nameof(EditActionText), typeof(string), typeof(DynamicDataGrid), new PropertyMetadata("Edit"));

        public string EditActionText
        {
            get { return (string) GetValue(EditActionTextProperty); }
            set { SetValue(EditActionTextProperty, value); }
        }
    }

    public partial class DynamicDataGrid
    {
        public static readonly DependencyProperty CanCreateActionProperty = DependencyProperty.Register(
            nameof(CanCreateAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteCreateItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CanRemoveActionProperty = DependencyProperty.Register(
            nameof(CanRemoveAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteRemoveItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CanUpdateActionProperty = DependencyProperty.Register(
            nameof(CanUpdateAction), typeof(Func<object, CanExecuteRoutedEventArgs, bool>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Func<object, CanExecuteRoutedEventArgs, bool>((o, args) =>
            {
                if (o is DynamicDataGrid d) d.CanExecuteUpdateItem(o, args);

                return args.CanExecute;
            })));

        public static readonly DependencyProperty CreateActionProperty = DependencyProperty.Register(
            nameof(CreateAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteCreateItem(o, args);
                })));

        public static readonly DependencyProperty RemoveActionProperty = DependencyProperty.Register(
            nameof(RemoveAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new FrameworkPropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteRemoveItem(o, args);
                })));

        public static readonly DependencyProperty EditActionProperty = DependencyProperty.Register(
            nameof(EditAction), typeof(Action<object, ExecutedRoutedEventArgs>), typeof(DynamicDataGrid),
            new PropertyMetadata(new Action<object, ExecutedRoutedEventArgs>(
                (o, args) =>
                {
                    if (o is DynamicDataGrid d) d.ExecuteUpdateItem(o, args);
                })));

        public Func<object, CanExecuteRoutedEventArgs, bool> CanCreateAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanCreateActionProperty);
            set => SetValue(CanCreateActionProperty, value);
        }

        public Func<object, CanExecuteRoutedEventArgs, bool> CanRemoveAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanRemoveActionProperty);
            set => SetValue(CanRemoveActionProperty, value);
        }

        public Func<object, CanExecuteRoutedEventArgs, bool> CanUpdateAction
        {
            get => (Func<object, CanExecuteRoutedEventArgs, bool>) GetValue(CanUpdateActionProperty);
            set => SetValue(CanUpdateActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> CreateAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(CreateActionProperty);
            set => SetValue(CreateActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> RemoveAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(RemoveActionProperty);
            set => SetValue(RemoveActionProperty, value);
        }

        public Action<object, ExecutedRoutedEventArgs> EditAction
        {
            get => (Action<object, ExecutedRoutedEventArgs>) GetValue(EditActionProperty);
            set => SetValue(EditActionProperty, value);
        }
    }
}