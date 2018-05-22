// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Grzybobranie;
using System.Diagnostics;

namespace Grzybki
{
    class Program
	{
		private static int amountOfTests = 7;

        static void Main(string[] args)
		{
			//launchGrzybki();
			launchZad4();
        }

		private static void launchZad4()
		{
			string fileName = "results2.txt";
			int polynomialRank = 3;
			ResultsFunction results = FileHelper.readResultsFromFile(fileName);
			MyMatrix matrix = MyMatrix.generateAproxMatrix(polynomialRank, results);
			//Console.Write(matrix.ToString());
			MyMatrix vector = MyMatrix.getVectorFromAproxMatrix(matrix);
			//Console.Write(vector.ToString());
			MyMatrix matrixForGauss = MyMatrix.createMatrixFromVector(polynomialRank, vector);
			//Console.Write(matrixForGauss);
			MyMatrix resultVector = MyMatrix.gauss(matrixForGauss, 1, false);
			Console.Write(resultVector);
		}

		private static void launchGrzybki()
		{
			for (int test = 0; test < amountOfTests; test++)
			{
				Data data = FileHelper.loadData("input/input" + test.ToString() + ".txt");
				Console.WriteLine(data);
				GameStateGenerator gameStateGenerator = new GameStateGenerator();
				gameStateGenerator.generateMatrix(data);
				List<GameState> allStates = gameStateGenerator.allStates;
				int size = allStates.Count;
				MyMatrix matrix = new MyMatrix(size, size);
				MyMatrix vector = new MyMatrix(size, 1);
				foreach (GameState state in allStates)
				{
					if (state.equation.Count == data.cubeSize)
					{
						foreach (GameState stateTmp in state.equation)
						{
							matrix[state.index, stateTmp.index] = (double)countStatesProbabilities(state.equation, stateTmp, data) / data.probabilitySum;
						}
					}
				}
				foreach (GameState state in allStates)
				{
					vector[state.index, 0] = state.win;
					if (state.win == 0)
					{
						matrix[state.index, state.index] = -1.0;
					}
					else if (state.win == 1)
					{
						matrix[state.index, state.index] = 1.0;
					}
				}

				File.WriteAllText("output/matrixOutput" + test.ToString() + ".txt", matrix.ToString());
				File.WriteAllText("output/vectorOutput" + test.ToString() + ".txt", vector.ToString());

				//GameState firstState = new GameState(1, data.playerOnePos, data.playerTwoPos);
				//MonteCarlo monteCarlo = new MonteCarlo(firstState, data);
				//Console.WriteLine(monteCarlo.simulate(1000000));

				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();
				MyMatrix result = MyMatrix.gauss(MyMatrix.matrixJoinVector(matrix, vector), 2, false);
				stopWatch.Stop();
				Console.WriteLine("{0} {1}", result[0, 0], stopWatch.Elapsed);

				stopWatch.Start();
				MyMatrix result2 = MyMatrix.gauss(MyMatrix.matrixJoinVector(matrix, vector), 2, true);
				stopWatch.Stop();
				Console.WriteLine("{0} {1}", result2[0, 0], stopWatch.Elapsed);

				stopWatch.Start();
				MyMatrix result3 = MyMatrix.jacobiIterative(new MyMatrix(matrix), new MyMatrix(vector));
				stopWatch.Stop();
				Console.WriteLine("{0} {1}", result3[0, 0], stopWatch.Elapsed);

				stopWatch.Start();
				MyMatrix result4 = MyMatrix.gaussSeidelIterative(new MyMatrix(matrix), new MyMatrix(vector));
				stopWatch.Stop();
				Console.WriteLine("{0} {1}", result4[0, 0], stopWatch.Elapsed);
			}
		}

        private static int countStatesProbabilities(List<GameState> equations, GameState state, Data data)
		{
            int result = 0;
            int i = 0;
            foreach (var stateTmp in equations)
			{
                if (stateTmp.Equals(state))
				{
                    result += data.cubeProbabilities[i];
                }
                i++;
            }
            return result;
        }
	}
}
