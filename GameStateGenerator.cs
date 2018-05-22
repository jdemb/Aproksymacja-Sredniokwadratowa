// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grzybki {
    public class GameStateGenerator
	{
        public List<GameState> allStates = new List<GameState>();

        int[] cubeValues;

        public void generateMatrix(Data data)
		{
            cubeValues = (int[])data.cubeValues.Clone();
            GameState firstState = new GameState(1, data.playerOnePos, data.playerTwoPos);
            firstState.index = 0;
            allStates.Add(firstState);
            generate();
        }
        public void generate()
		{
            int lowerBound = 0;
            int higherBound;
            while (checkStateReadStatus())
			{
                higherBound = allStates.Count;

                for (int i = lowerBound; i < higherBound; i++)
				{
                    GameState tmpState = allStates[i];

                    if (tmpState.posOne != 0 && tmpState.posTwo != 0)
					{
                        for (int j = 0; j < cubeValues.Length; j++)
						{
                            int One = tmpState.posOne;
                            int Two = tmpState.posTwo;
                            if (tmpState.turn == 1)
							{
                                One += cubeValues[j];
                            } else if (tmpState.turn == 0)
							{
                                Two += cubeValues[j];
                            }
                            GameState tmp = new GameState(tmpState.nextTurn(), One, Two);
                            checkIfGlobalAlreadyContain(tmpState, tmp);
                        }
                    }
					else if (tmpState.posOne == 0)
                        tmpState.win = 1;
                    tmpState.readStatus = 1;
                }
                lowerBound = higherBound;
            }
        }
        public void checkIfGlobalAlreadyContain(GameState state, GameState tmp)
		{ 
            if (!allStates.Contains(tmp))
			{
                tmp.index = allStates.Count;
                allStates.Add(tmp);
                state.addToEquation(tmp);
            }
			else
			{
                int founded = allStates.IndexOf(tmp);
                state.addToEquation(allStates[founded]); //state.equations.add(...)
            }
        }

        public bool checkStateReadStatus()
		{
            foreach (GameState state in allStates)
			{
                if (state.readStatus == 0)
				{
                    return true;
                }
            }
            return false;
        }
    }
}
