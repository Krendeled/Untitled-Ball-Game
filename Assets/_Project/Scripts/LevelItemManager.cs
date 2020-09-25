using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledBallGame
{
    public class LevelItemManager
    {
        public IEnumerable<LevelItem> GetAll()
        {
            return Resources.LoadAll<LevelItem>("_Project/LevelItems/");
        }
    }
}