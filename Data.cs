// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grzybki
{
    public class Data
	{
		#region variables
		public int finishIndex;
        public int mapSize;
        public int mushroomCount;
        public int playerOnePos;
        public int playerTwoPos;
        public int cubeSize;
        public int probabilitySum;
        public int[] mushroomMap;
        public int[] cubeValues;
        public int[] cubeProbabilities;
		#endregion

		public Data(int mapSize, int mushroomCount, int playerOnePos, int playerTwoPos, int cubeSize,
            int probabilitySum, int[] cubeValues, int[] cubeProbabilities, int[] mushroomMap)
		{
            this.mapSize = mapSize;
            this.mushroomCount = mushroomCount;
            this.playerOnePos = playerOnePos;
            this.playerTwoPos = playerTwoPos;
            this.cubeSize = cubeSize;
            this.probabilitySum = probabilitySum;
            this.cubeValues = (int[])cubeValues.Clone();
            this.cubeProbabilities = (int[])cubeProbabilities.Clone();
            this.mushroomMap = (int[])mushroomMap.Clone();
            this.finishIndex = 0; 
            GameState.mapSize = mapSize; 
        }

        public override string ToString()
		{
            string cubeVals = "", cubeProbs = "", mushrooms = "";
            for (int i = 0; i < cubeSize; i++)
			{
                cubeVals += cubeValues[i] + " ";
                cubeProbs += cubeProbabilities[i] + " ";
            }
            for (int i = 0; i < mapSize; i++)
			{
                mushrooms += mushroomMap[i] + " ";
            }
            return "mapSize : " + mapSize + "\nmushroomCount : " + mushroomCount +
                "\nplayerOnePos : " + playerOnePos + "\nplayerTwoPos : " + playerTwoPos +
                "\ncubeSize : " + cubeSize + "\nprobabilitySum : " + probabilitySum +
                "\ncubeValues : " + cubeVals + "\ncubeProbability : " + cubeProbs +
                "\nmushroomMap : " + mushrooms; // "\nfinishIndex: " + finishIndex;
        }
    }
}
