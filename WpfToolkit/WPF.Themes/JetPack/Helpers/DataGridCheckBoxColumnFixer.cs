﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF.Themes.JetPack.Helpers
{
    public class DataGridCheckBoxColumnFixer : DependencyObject
    {
        #region Fix

        /// <summary>
        /// Fix Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FixProperty =
            DependencyProperty.RegisterAttached("Fix", typeof(bool), typeof(DataGridCheckBoxColumnFixer),
                new FrameworkPropertyMetadata((bool)false, OnFixChanged));

        private static Style _defaultElementStyle;
        private static Style _defaultEditingElementStyle;

        /// <summary>
        /// Gets the Fix property. This dependency property 
        /// indicates ....
        /// </summary>
        public static bool GetFix(DependencyObject d)
        {
            return (bool)d.GetValue(FixProperty);
        }

        /// <summary>
        /// Sets the Fix property. This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetFix(DependencyObject d, bool value)
        {
            d.SetValue(FixProperty, value);
        }

        /// <summary>
        /// Handles changes to the Fix property.
        /// </summary>
        private static void OnFixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldFix = (bool)e.OldValue;
            bool newFix = (bool)d.GetValue(FixProperty);

            if (d == null) return;


            DataGrid dataGrid = d as DataGrid;
            if (oldFix) Unsubscribe(dataGrid);
            if (!newFix) return;
            if (dataGrid == null) return;
            if (!dataGrid.AutoGenerateColumns) return;

            dataGrid.AutoGeneratingColumn += DataGridAutoGeneratingColumn;
            dataGrid.AutoGeneratedColumns += DataGridAutoGeneratedColumns;

        }

        static void DataGridAutoGeneratedColumns(object sender, EventArgs e)
        {
            var dataGrid = sender as DataGrid;
            Unsubscribe(dataGrid);
        }

        private static void Unsubscribe(DataGrid dataGrid)
        {
            if (dataGrid != null)
            {
                dataGrid.AutoGeneratingColumn -= DataGridAutoGeneratingColumn;
                dataGrid.AutoGeneratedColumns -= DataGridAutoGeneratedColumns;
            }
        }

        public static Style DefaultElementStyle
        {
            get
            {
                if (_defaultElementStyle == null)
                {
                    Style baseStyle = Application.Current.FindResource(typeof(CheckBox)) as Style;

                    Style style;
                    if (baseStyle != null)
                    {
                        style = new Style(typeof(CheckBox), baseStyle);
                    }
                    else
                    {
                        style = new Style(typeof(CheckBox));
                    }
                    
                    style.Setters.Add(new Setter(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                    style.Setters.Add(new Setter(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Seal();
                    _defaultElementStyle = style;
                }

                return _defaultElementStyle;
            }
        }


        public static Style DefaultEditingElementStyle
        {
            get
            {
                if (_defaultEditingElementStyle == null)
                {
                    Style baseStyle = Application.Current.FindResource(typeof(CheckBox)) as Style;

                    Style style;
                    if (baseStyle != null)
                    {
                        style = new Style(typeof(CheckBox), baseStyle);
                    }
                    else
                    {
                        style = new Style(typeof(CheckBox));
                    }


                    // When not in edit mode, the end-user should not be able to toggle the state
                    style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
                    style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
                    style.Setters.Add(new Setter(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                    style.Setters.Add(new Setter(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Seal();
                    _defaultEditingElementStyle = style;
                }

                return _defaultEditingElementStyle;
            }
        }

        static void DataGridAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!(e.Column is DataGridCheckBoxColumn)) return;

            var checkBoxColumn = e.Column as DataGridCheckBoxColumn;

            checkBoxColumn.ElementStyle = DefaultElementStyle;
            checkBoxColumn.EditingElementStyle = DefaultEditingElementStyle;
        }

        #endregion



    }
}
