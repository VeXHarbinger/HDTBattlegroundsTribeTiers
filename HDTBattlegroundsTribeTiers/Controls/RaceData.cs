using HearthDb.Enums;
using System.Collections.Generic;
using System.Linq;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;

namespace HDTBattlegroundsTribeTiers.Controls
{
    public class RaceData
    {
        public List<RaceGroupData> raceGroupData;

        public RaceData(List<RaceGroupData> raceData)
        {
            raceGroupData = raceData;
        }

        public Race SelectedRace { get; set; }

        public RaceGroupData getRaceData(Race r)
        {
            return raceGroupData.First(d => d.race == r);
        }
    }

    public class RaceGroupData
    {
        public Race race;
        public List<RaceTierData> raceTierData;
    }
}