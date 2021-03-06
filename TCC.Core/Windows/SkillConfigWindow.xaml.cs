﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GongSolutions.Wpf.DragDrop;
using TCC.Data;
using TCC.Data.Abnormalities;
using TCC.Data.Skills;
using TCC.ViewModels;

namespace TCC.Windows
{
    /// <summary>
    /// Logica di interazione per SkillConfigWindow.xaml
    /// </summary>
    public partial class SkillConfigWindow
    {

        public IntPtr Handle { get; private set; }
        private CooldownWindowViewModel _vm { get;  }
        public SkillConfigWindow()
        {
            InitializeComponent();
            DataContext = WindowManager.CooldownWindow.VM;
            _vm = DataContext as CooldownWindowViewModel;

            Closing += OnClosing;
            Loaded += (_, __) => Handle = new WindowInteropHelper(this).Handle;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if(Opacity != 0) e.Cancel = true;
            ClosewWindow(null,null);
        }

        public class GenericDragHandler : IDropTarget
        {
            public void DragOver(IDropInfo dropInfo)
            {
            }

            public void Drop(IDropInfo dropInfo)
            {
            }
        }

        public GenericDragHandler DragHandler => new GenericDragHandler();

        public static bool IsOpen { get; private set; }

        private void ClosewWindow(object sender, RoutedEventArgs e)
        {

            var an = new DoubleAnimation(0, TimeSpan.FromMilliseconds(200));
            FocusManager.ForceFocused = false;
            WindowManager.ForegroundManager.ForceUndim = false;
            WindowManager.SkillConfigWindow = null;

            an.Completed += (s, ev) =>
            {
                Close();
                IsOpen = false;
                if (Settings.SettingsHolder.ForceSoftwareRendering) RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            };
            BeginAnimation(OpacityProperty, an);
            _vm.Save();
        }

        internal void ShowWindow()
        {
            if (Settings.SettingsHolder.ForceSoftwareRendering) RenderOptions.ProcessRenderMode = RenderMode.Default;
            FocusManager.ForceFocused = true;
            WindowManager.ForegroundManager.ForceUndim = true;
            WindowManager.SkillConfigWindow = this;
            Dispatcher.Invoke(() =>
            {
                IsOpen = true;
                var animation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200));
                if (IsVisible) return;
                Opacity = 0;
                Show();
                Activate();
                BeginAnimation(OpacityProperty, animation);
            });
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SkillSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var view = ((ICollectionView)_vm.SkillsView);
            view.Filter = o =>  ((Skill)o).ShortName.IndexOf(((TextBox) sender).Text, StringComparison.InvariantCultureIgnoreCase) != -1;
            view.Refresh();
        }

        private void ItemSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var view = (ICollectionView)_vm.ItemsView;
            view.Filter = o => ((Item)o).Name.IndexOf(((TextBox)sender).Text, StringComparison.InvariantCultureIgnoreCase) != -1;
            view.Refresh();
        }
        private void PassivitySearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var view = (ICollectionView)_vm.AbnormalitiesView;
            view.Filter = o => ((Abnormality)o).Name.IndexOf(((TextBox)sender).Text, StringComparison.InvariantCultureIgnoreCase) != -1;
            view.Refresh();
        }

        private void RemoveHiddenSkill(object sender, RoutedEventArgs e)
        {
            _vm.RemoveHiddenSkill(((Button) sender).DataContext as Cooldown);
        }
    }
}
