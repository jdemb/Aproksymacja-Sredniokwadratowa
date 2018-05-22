// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1

using Grzybki;
using System;

namespace Grzybobranie
{
	public class MonteCarlo
	{
		private Data data;
		private GameState firstState;
		private Random random;


		public MonteCarlo(GameState firstState, Data data)
		{
			this.data = data;
			this.firstState = firstState;
			random = new Random();
		}

		public double simulate(int simulations)
		{
			int counter = 0;
			GameState current;
			for (int i = 0; i < simulations; i++)
			{
				current = firstState;
				while (current.end != true)
				{
					int first = current.posOne;
					int second = current.posTwo;
					if (current.turn == 1)
						first += rollTheDice();
					else if (current.turn == 0)
						second += rollTheDice();
					current = new GameState(current.nextTurn(), first, second);
					checkIfEnd(current);
				}
				if (current.win == 1)
					counter++;
			}
			return (double)counter / simulations;
		}

		private void checkIfEnd(GameState state)
		{
			if (state.posOne == 0)
			{
				state.win = 1;
				state.end = true;
			}
			else if (state.posTwo == 0)
			{
				state.end = true;
				state.win = 0;
			}
		}

		private int rollTheDice()
		{
			int generatedValue = modulo(randomInteger(random), data.probabilitySum);
			int lowerBound = 0;
			int higherBound = data.cubeProbabilities[0];
			for (int i = 0; i < data.cubeProbabilities.Length; i++)
			{
				if (generatedValue >= lowerBound && generatedValue < higherBound)
				{
					return data.cubeValues[i];
				}
				lowerBound += data.cubeProbabilities[i];
				higherBound += data.cubeProbabilities[i + 1];
			}
			return 0;
		}

		private static int randomInteger(Random random)
		{
			return random.Next();
		}

		private static int modulo(int x, int m)
		{
			int r = x % m;
			return r < 0 ? r + m : r;
		}
	}
}
