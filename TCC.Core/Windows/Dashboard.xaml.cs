﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TCC.Data.Pc;
using TCC.Data.Skills;
using TCC.ViewModels;

namespace TCC.Windows

{
    /// <summary>
    /// Logica di interazione per Dashboard.xaml
    /// </summary>
    public partial class Dashboard : TccWindow
    {
        public DashboardViewModel VM { get; }
        public IntPtr Handle { get; private set; }

        public Dashboard()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
            VM = DataContext as DashboardViewModel;
            Loaded += (_, __) => Handle = new WindowInteropHelper(this).Handle;
            Showed += () => VM.UpdateBuffs();
            Hidden += () => SessionManager.CurrentDatabase.DungeonDatabase.SaveCustomDefs();
            //MouseLeftButtonDown += (_, __) => MenuPopup.IsOpen = false;
        }


        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            HideWindow();
            VM.SaveCharacters();
        }

        private void OnTitleBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void OnDetailsMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var an = new DoubleAnimation(0, TimeSpan.FromSeconds(.3)) { EasingFunction = R.MiscResources.QuadraticEase };
            an.Completed += (o, args) => DetailsBorder.IsHitTestVisible = false;
            DetailsBorder.BeginAnimation(OpacityProperty, an);
        }

        public void ShowDetails()
        {
            InventoryFilter.Clear();
            var an = new DoubleAnimation(1, TimeSpan.FromSeconds(.3)) { EasingFunction = R.MiscResources.QuadraticEase };
            an.Completed += (o, args) => DetailsBorder.IsHitTestVisible = true;
            DetailsBorder.BeginAnimation(OpacityProperty, an);
        }

        private void FilterInventory(object sender, TextChangedEventArgs e)
        {
            var view = ((ICollectionView)VM.SelectedCharacterInventory);
            view.Filter = o =>
            {
                var item = ((InventoryItem)o).Item;
                var name = item.Name;
                return name.IndexOf(((TextBox)sender).Text,
                               StringComparison.InvariantCultureIgnoreCase) != -1;
            };
            view.Refresh();
        }
        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0 && e.AddedItems[0] is TabItem) OnDetailsMouseButtonDown(null, null);
        }

        private void RemoveCharacter(object sender, RoutedEventArgs e)
        {
            OnDetailsMouseButtonDown(null, null);
            //VM.Characters.Remove(VM.SelectedCharacter);
            VM.SelectedCharacter.Hidden = true;
        }
        private void OpenMergedInventory(object sender, RoutedEventArgs e)
        {
            new MergedInventoryWindow() { Topmost = true, Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner }.ShowDialog();
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            //MenuPopup.IsOpen = true;
        }

        private void UnHideChar(object sender, RoutedEventArgs e)
        {
            ((Character)((FrameworkElement)sender).DataContext).Hidden = false;
            //HiddenCharsPopup.IsOpen = false;
        }

        private void OpenHiddenCharsPopup(object sender, MouseEventArgs e)
        {
            //HiddenCharsPopup.IsOpen = true;
        }

        private void CloseHiddenCharsPopup(object sender, MouseEventArgs e)
        {
            var s = sender as FrameworkElement;
            Task.Delay(1000).ContinueWith(t =>
            {
            //    if (!s.IsMouseOver && !HiddenCharsPopup.IsMouseOver) Dispatcher.Invoke(() => HiddenCharsPopup.IsOpen = false);
            });
        }
    }
}
