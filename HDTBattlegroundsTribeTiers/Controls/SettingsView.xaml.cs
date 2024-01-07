using HDTBattlegroundsTribeTiers.Logic;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace HDTBattlegroundsTribeTiers.Controls
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ScrollViewer
    {
        public static bool IsUnlocked { get; private set; }

        public InputMoveManager inputMoveManager;

        private static Flyout _flyout;

        // ToDo: The window shouldn't be statically named
        private static string panelName = "TribalCouncilView";

        public SettingsView()
        {
            InitializeComponent();
            initTranslation();
        }

        public static Flyout Flyout
        {
            get
            {
                if (_flyout == null)
                {
                    _flyout = CreateSettingsFlyout();
                }
                return _flyout;
            }
        }

        public IEnumerable<Orientation> OrientationTypes => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();

        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout();
            settings.Position = Position.Left;
            Panel.SetZIndex(settings, 100);
            settings.Header = LocalizeTools.GetLocalized("LabelSettings");
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        private void BtnShowHide_Click(object sender, RoutedEventArgs e)
        {
            RacesDisplayControl racesViewer = Core.OverlayCanvas.FindChild<RacesDisplayControl>(panelName);
            if (racesViewer != null)
            {
                inputMoveManager = new InputMoveManager(racesViewer);
                bool IsVis = (racesViewer.Visibility == Visibility.Visible);
                BtnShowHide.Content = IsVis ? LocalizeTools.GetLocalized("LabelHide") : LocalizeTools.GetLocalized("LabelShow");
                racesViewer.Visibility = IsVis ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            RacesDisplayControl racesViewer = Core.OverlayCanvas.FindChild<RacesDisplayControl>(panelName);
            if (racesViewer != null)
            {
                inputMoveManager = new InputMoveManager(racesViewer);
                IsUnlocked = inputMoveManager.Toggle();
                BtnShowHide.IsEnabled = !IsUnlocked;
                BtnUnlock.Content = IsUnlocked ? LocalizeTools.GetLocalized("LockLabel") : BtnUnlock.Content = LocalizeTools.GetLocalized("UnlockLabel");

                if (IsUnlocked && (racesViewer.Visibility != Visibility.Visible))
                {
                    racesViewer.Visibility = Visibility.Visible;
                    BtnShowHide.Content = LocalizeTools.GetLocalized("LabelHide");
                }
            }
        }

        /// <summary>
        /// Does our default translation, just till I fix the XAML Hooks.
        /// </summary>
        public void initTranslation()
        {
            BtnUnlock.Content = LocalizeTools.GetLocalized("LabelUnlock");
            BtnShowHide.Content = LocalizeTools.GetLocalized("LabelShow");
            LblOpacity.Content = LocalizeTools.GetLocalized("LabelOpacity");
            LblScale.Content = LocalizeTools.GetLocalized("LabelScale");
        }
    }
}