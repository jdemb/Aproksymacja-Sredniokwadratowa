// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Grzybki
{
    public class FileHelper
	{

        public static Data loadData(string filename)
		{
			#region variables
			int size;
			int mushroomCount;
			int player1StartIndex;
			int player2StartIndex;
			int cubeSize;
			int probabilitySum = 0;
			int[] mushroomMap;
			int[] cubeValues;
			int[] cubeProbabilities;
			#endregion

			if (!File.Exists(filename))
			{
                throw new FileNotFoundException("Could not open file " + filename + " !");
            }

			#region readFromFileToVariables
			string[] data = File.ReadAllLines(filename);
            size = Int32.Parse(data[0]) * 2 + 1;
            string[] scanner = data[1].Split(' ');
            mushroomCount = Int32.Parse(scanner[0]);
            mushroomMap = new int[size];
            for (int i = 0; i < mushroomCount; i++)
			{
                mushroomMap[Int32.Parse(scanner[i + 1])] = 1;
            }
            scanner = data[2].Split(' ');
            player1StartIndex = Int32.Parse(scanner[0]);
            player2StartIndex = Int32.Parse(scanner[1]);
            cubeSize = Int32.Parse(data[3]);
            cubeValues = new int[cubeSize];
            cubeProbabilities = new int[cubeSize];
            GameState.mapSize = size;
            scanner = data[4].Split(' ');
            for (int i = 0; i < cubeSize; i++)
			{
                cubeValues[i] = Int32.Parse(scanner[i]);
            }
            scanner = data[5].Split(' ');
            for (int i = 0; i < cubeSize; i++)
			{
                cubeProbabilities[i] = Int32.Parse(scanner[i]);
                probabilitySum += cubeProbabilities[i];
            }
			#endregion

			if (mushroomMap[player1StartIndex] == 1 || mushroomMap[player2StartIndex] == 1)
			{
                throw new Exception("Grzyb nie moze stac na pozycji startowej gracza 1 / gracza 2!");
            }

            Data dataFromFile = new Data(size, mushroomCount, player1StartIndex, player2StartIndex, cubeSize,
                probabilitySum, cubeValues, cubeProbabilities, mushroomMap);

            return dataFromFile;
        }

        public static void saveMatrix(string filename, MyMatrix matrix)
		{
			File.WriteAllText(filename, matrix.ToString());
            return;
        }

		public static ResultsFunction readResultsFromFile(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException("Could not open file " + fileName + " !");
			}
			string[] data = File.ReadAllLines(fileName);
			ResultsFunction results = new ResultsFunction();
			for (int i = 0; i < data.Length; i++)
			{
				string[] scanner = data[i].Split(' ');
				results.x.Add(Int32.Parse(scanner[0]));
				results.y.Add(Int32.Parse(scanner[1]));
			}
			return results;
		}
    }
}
