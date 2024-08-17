using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGConsole.GameLogic
{
    public class FightScene
    {
        private char[,] CombatMap;
        public FightScene()
        {
        }
        private void PrintScene()
        {
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    CombatMap[i, j] = '*';
                }
            }
        }
    }
}
