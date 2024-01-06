using HDTBattlegroundsTribeTiers.Logic;
using HDTBattlegroundsTribeTiers.Properties;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Controls.Overlay.Battlegrounds.Session;
using Hearthstone_Deck_Tracker.Hearthstone;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace HDTBattlegroundsTribeTiers.Controls
{
    public class TribalCouncil
    {
        private Lazy<BattlegroundsDb> _db = new();

        private BattlegroundsSessionViewModel bsvm;

        private static string panelName = "TribalCouncilView";

        public static InputMoveManager inputMoveManager;

        public RacesDisplayControl RacesDisplay;

        public TribalCouncil()
        {
            init();
        }

        private void getPartnerDisplay()
        {
            //if (RacesDisplay is not null)
            //{
            //    Core.OverlayCanvas.Children.Add(RacesDisplay);
            //}
        }

        private void OnMouseOff()
        {
            if (RacesDisplay is not null)
            {
                RacesDisplay.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void PlayerHandMouseOver(Card card)
        {
            if (RacesDisplay is not null)
            {
                RacesDisplay.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (RacesDisplay is not null)
            {
                RacesDisplay.RenderTransform = new ScaleTransform(Settings.Default.Scale / 100, Settings.Default.Scale / 100);
                RacesDisplay.Opacity = Settings.Default.Opacity / 100;
            }
        }

        public void init()
        {
            SetTribalDisplay();
            HashSet<Race> AvailableRaces = BattlegroundsUtils.GetAvailableRaces(Core.Game.CurrentGameStats?.GameId);
            List<int> AvailableTiers = BattlegroundsUtils.GetAvailableTiers(null).ToList();

            if ((AvailableTiers is not null && AvailableTiers.Count > 0) &&
                (AvailableRaces is not null && AvailableRaces.Count > 0))
            {
                LoadTiers(AvailableTiers, AvailableRaces);
                Core.OverlayCanvas.Children.Add(RacesDisplay);

                Settings.Default.PropertyChanged += SettingsChanged;
                SettingsChanged(null, null);

                Canvas.SetTop(RacesDisplay, Settings.Default.Top);
                Canvas.SetRight(RacesDisplay, Settings.Default.Right);

                inputMoveManager = new InputMoveManager(RacesDisplay);

                // Just for testing
                GameEvents.OnPlayerHandMouseOver.Add(PlayerHandMouseOver);
                GameEvents.OnMouseOverOff.Add(OnMouseOff);
            }

            // bsvm = Core.Game.BattlegroundsSessionViewModel;
        }

        public void LoadTiers(List<int> AvailableTiers, HashSet<Race> AvailableRaces)
        {
            foreach (Race r in AvailableRaces)
            {
                RaceDisplayControl RaceDisplay = new RaceDisplayControl();
                RaceDisplay.RaceGroup.Text = r.ToString();
                foreach (int t in AvailableTiers)
                {
                    RaceDisplay.RaceViewer.Children.Add(LoadTribeByTier(t, r));
                }
                RacesDisplay.RacesViewer.Children.Add(RaceDisplay);
            }
        }

        public TribeTierDisplay LoadTribeByTier(int tier, Race race)
        {
            List<Card> cards = _db.Value.GetCards(tier, race);
            TribeTierDisplay ttd = new();
            ttd.TierLevel.Text = "Tier " + tier.ToString();
            ttd.TribeCards.Update(cards, true);
            // ToDo: Make Collapsed
            ttd.Visibility = System.Windows.Visibility.Visible;
            return ttd;
        }

        public void SetTribalDisplay()
        {
            ScrollViewer msc = Core.OverlayCanvas.FindChild<ScrollViewer>("MinionScrollViewer");
            ItemsControl gc = msc.FindChild<ItemsControl>("GroupsControl");
           
            
            
            RacesDisplay = new()
            {
                Name= panelName,
                Height = 400
                //Height = msc.Height > 0 ? msc.Height - 100 : 300,
                //Width = msc.Width > 0 ? msc.Width : 190
            };
        }

        public void SliderClosed()
        { }

        public void SliderOpen()
        { }
    }
}