using HDTBattlegroundsTribeTiers.Controls;
using HDTBattlegroundsTribeTiers.Logic;
using HDTBattlegroundsTribeTiers.Properties;
using Hearthstone_Deck_Tracker.API;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace HDTBattlegroundsTribeTiers
{
    /// <summary>
    /// This is where we put the logic for our Plug-in
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class HDTBattlegroundsTribeTiers : IDisposable
    {
        public TribalCouncil tribalCouncil;
        public static InputMoveManager inputMoveManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HDTBattlegroundsTribeTiers"/> class.
        /// </summary>
        public HDTBattlegroundsTribeTiers()
        {
            // We are adding the Panel here for simplicity.  It would be better to add it under InitLogic()
            InitLogic();

            GameEvents.OnGameStart.Add(GameTypeCheck);
            GameEvents.OnGameEnd.Add(CleanUp);
        }

        /// <summary>
        /// Check the game type to see if our Plug-in is used.
        /// </summary>
        private void GameTypeCheck()
        {
            InitLogic();

            // ToDo : Enable toggle Props
            if (Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_BATTLEGROUNDS ||
                Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_BATTLEGROUNDS_FRIENDLY ||
                Core.Game.CurrentGameType == HearthDb.Enums.GameType.GT_BATTLEGROUNDS_AI_VS_AI)
            {
                //InitLogic();
            }
        }

        private void InitLogic()
        {
            tribalCouncil = new TribalCouncil();
        }

        public void CleanUp()
        {
            if (tribalCouncil != null)
            {
               Core.OverlayCanvas.Children.Remove(tribalCouncil.RacesDisplay);
            }
            tribalCouncil = null;
        }

        public void Dispose()
        {
            CleanUp();
            inputMoveManager.Dispose();
        }
    }
}