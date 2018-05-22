// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grzybki
{
    public class GameState
	{
		#region variables
		public static int mapSize; 
        public int index;
        public int turn; //1 -> gracz 1, 0 -> tura gracza 2
        public int posOne;
        public int posTwo;
        public List<GameState> equation;
        public int win;
		public bool end;
        public int readStatus;
		#endregion

		public GameState(int turn, int posOne, int posTwo)
		{
            this.turn = turn;
            this.posOne = mod(posOne,mapSize); 
            this.posTwo = mod(posTwo, mapSize);
            equation = new List<GameState>();
        }

        public int nextTurn()
		{
            if (turn == 1)
                return 0;
            else
                return 1;
        }

        public void addToEquation(GameState gameState)
		{
            equation.Add(gameState);
        }

        public static int mod(int x, int m)
		{
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public override bool Equals(object obj)
		{
            if (this == obj) return true;
            if (obj == null || obj.GetType() != GetType()) return false;
            GameState gameState = (GameState)obj;
            if (turn != gameState.turn) return false;
            if (posOne != gameState.posOne) return false;
            if (posTwo != gameState.posTwo) return false;
            return true;
        }

        public override string ToString()
		{
            return base.ToString();
        }

        public override int GetHashCode()
		{
            return base.GetHashCode();
        }
    }
}
