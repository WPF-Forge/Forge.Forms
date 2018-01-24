using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bindables;
using Forge.Forms.Annotations;
using Forge.Forms.Controls;
using Humanizer;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using MoreLinq;

namespace Forge.Forms.Collections.Controls
{
    public class CrudControl : ContentControl
    {
        [DependencyProperty(Options = FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnPropertyChanged = nameof(ItemsSourceChanged))]
        public object ItemsSource { get; set; }

        private DataGrid DataGrid { get; } = new DataGrid {Height = 300};

        private StackPanel StackPanel { get; } = new StackPanel();

        private Action OnClick { get; set; }

        public CrudControl()
        {
            var content = new Button {Content = "Open dialog"};
            content.Click += (sender, args) => OnClick?.Invoke();
            DataGrid.AutoGenerateColumns = true;
            DataGrid.AutoGeneratingColumn += DataGridOnAutoGeneratingColumn;
            StackPanel.Children.Add(content);
            StackPanel.Children.Add(DataGrid);
            Content = StackPanel;
        }

        private void DataGridOnAutoGeneratingColumn(object o, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                var fieldAttribute = descriptor.Attributes.OfType<FieldAttribute>().FirstOrDefault();
                if (fieldAttribute != null)
                {
                    e.Column.Header = fieldAttribute.Name;
                    if (fieldAttribute.IsVisible is bool b)
                        e.Column.Visibility = b ? Visibility.Visible : Visibility.Collapsed;
                }

                e.Column.Header = (e.Column.Header as string ?? descriptor.Name).Humanize();
            }
        }

        public void OpenDialog(object form)
        {
            this.Invoke(() => DialogHost.OpenDialogCommand.Execute(form, null));
        }

        public static void ItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is CrudControl crudControl && crudControl.ItemsSource is IEnumerable enumerable)
            {
                var itemsList = enumerable.OfType<object>().ToList();
                var itemsTypes = itemsList.DistinctBy(i => i.GetType()).Select(i => i.GetType()).ToList();
                crudControl.DataGrid.ItemsSource = itemsList;

                var forms = new List<DynamicFormWrapper>();

                foreach (var type in itemsTypes)
                {
                    forms.Add(new DynamicFormWrapper(Activator.CreateInstance(type), null, DialogOptions.Default));
                }

                if (forms.Count == 1)
                {
                    crudControl.OnClick = () => { crudControl.OpenDialog(forms.First()); };
                }
            }
        }
    }
}