// Jędrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grzybki
{
    public class MyMatrix
	{
		private double[,] matrix;

		private static readonly double epsilon = 1E-15;

        public MyMatrix(int rows, int columns) {
            matrix = new double[rows, columns];
        }
		public MyMatrix(double[,] array) {
            matrix = new double[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    matrix[i, j] = array[i, j];
                }
            }
        }
		public MyMatrix(double[] array) {
            matrix = new double[array.GetLength(0), 1];
            for (int i = 0; i < array.GetLength(0); i++) {
                matrix[i, 0] = array[i];
            }
        }
		public MyMatrix(MyMatrix m) {
            matrix = new double[m.rowCount, m.columnCount];
            for (int i = 0; i < m.rowCount; i++) {
                for (int j = 0; j < m.columnCount; j++) {
                    matrix[i, j] = m[i, j];
                }
            }
        }

        public double this[int rowIndex, int columnIndex] {
            set { matrix[rowIndex, columnIndex] = value; }
            get { return matrix[rowIndex, columnIndex]; }
        }

        public int rowCount {
            get { return matrix.GetLength(0); }
        }
		public int columnCount {
            get { return matrix.GetLength(1); }
        }

		public static MyMatrix operator -(MyMatrix a, MyMatrix b)
		{
			if (a.rowCount != b.rowCount || a.columnCount != b.columnCount)
			{
				Console.WriteLine("Matrices sizes are incorrect for operation : a - b!");
				return null;
			}

			MyMatrix c = new MyMatrix(a.rowCount, a.columnCount);
			for (int i = 0; i < a.rowCount; i++)
			{
				for (int j = 0; j < a.columnCount; j++)
				{
					c.matrix[i, j] = a.matrix[i, j] - b.matrix[i, j];
				}
			}
			return c;
		}
		public static MyMatrix operator +(MyMatrix a, MyMatrix b) {
            if (a.rowCount != b.rowCount || a.columnCount != b.columnCount) {
                Console.WriteLine("Matrices sizes are incorrect for operation : a + b!");
                return null;
            }

            MyMatrix c = new MyMatrix(a.rowCount, a.columnCount);
            for (int i = 0; i < a.rowCount; i++) {
                for (int j = 0; j < a.columnCount; j++) {
                    c.matrix[i, j] = a.matrix[i, j] + b.matrix[i, j];
                }
            }
            return c;
        }
		public static MyMatrix operator *(MyMatrix a, MyMatrix b) {
            if (a.columnCount != b.rowCount) {
                Console.WriteLine("Matrices sizes are incorrect for operation : a * b!");
                return null;
            }
            MyMatrix c = new MyMatrix(a.rowCount, b.columnCount);
            for (int i = 0; i < a.rowCount; i++) {
                for (int j = 0; j < b.columnCount; j++) {
                    c.matrix[i, j] = default(double);
                    for (int k = 0; k < b.rowCount; k++) {
                        c.matrix[i, j] += a.matrix[i, k] * b.matrix[k, j];
                    }
                }
            }
            return c;
        }
		public static MyMatrix gauss(MyMatrix AB, int version, bool optimise) {
            if (AB.rowCount != AB.columnCount - 1)
                throw new Exception("Matrix N x (N+1) is required for Gaussian Eliminations!");
            int n = AB.rowCount;
            List<int> queue = new List<int>();
            if (version == 1) { //base
                for (int i = 0; i < n; i++) {
                    for (int k = i + 1; k < n; k++) {
                        double c = (default(double) - AB[k, i]) / AB[i, i];
                        for (int j = i; j < n + 1; j++) {
                            if (i == j)
                                AB[k, j] = default(double);
                            else
                                AB[k, j] += c * (dynamic)AB[i, j];
                        }
                    }
                }
            } else if (version == 2) { //partial
                for (int i = 0; i < n; i++) {
                    double max = Math.Abs(AB[i, i]);
                    int maxRow = i;
                    for (int k = i + 1; k < n; k++)
					{
                        if (Math.Abs(AB[k, i]) > max) {
                            max = Math.Abs(AB[k, i]);
                            maxRow = k;
                        }
                    }
                    for (int k = i; k < n + 1; k++) {
                        double tmp = AB[maxRow, k];
                        AB[maxRow, k] = AB[i, k];
                        AB[i, k] = tmp;
                    }

                    for (int k = i + 1; k < n; k++) {
                        double c = (default(double) - AB[k, i]) / AB[i, i];
                        if (optimise == true && c == default(double))
                            continue;
                        for (int j = i; j < n + 1; j++) {
                            if (i == j)
                                AB[k, j] = default(double);
                            else
                                AB[k, j] += c * AB[i, j];
                        }

                    }
                }
            } else if (version == 3) { //full
                for (int i = 0; i < n; i++) {
                    queue.Add(i);
                }
                for (int i = 0; i < n; i++) {
                    findBiggestValue(AB, i, queue);
                    for (int k = i + 1; k < n; k++) {
                        double c = (default(double) - AB[k, i]) / AB[i, i];

                        for (int j = i; j < n + 1; j++) {
                            if (i == j)
                                AB[k, j] = default(double);
                            else
                                AB[k, j] += c * AB[i, j];
                        }
                    }
                }
            } else {
                throw new Exception("Unknown Gauss elimination version! 1 - base, 2 - partial, 3 - full");
            }
            MyMatrix X = countVariables(AB);
            if (version == 3) X.resultByQueue(X, queue);
            return X;
        }
		public static MyMatrix matrixJoinVector(MyMatrix A, MyMatrix B)
		{
			if (A.rowCount != B.rowCount)
			{
				throw new Exception("Wrong Matrixes size!");
			}
			if (B.columnCount != 1)
			{
				throw new Exception("Wrong Vector columnCount!");
			}
			MyMatrix result = new MyMatrix(A.rowCount, A.columnCount + 1);
			int j = 0;
			for (int i = 0; i < A.rowCount; i++)
			{
				for (j = 0; j < A.columnCount; j++)
				{
					result[i, j] = A[i, j];
				}
			}

			for (int i = 0; i < A.rowCount; i++)
			{
				result[i, j] = B[i, 0];
			}
			return result;
		}
		public static MyMatrix jacobiIterative(MyMatrix A, MyMatrix B)
		{
			int n = A.rowCount;
			if (A.rowCount != B.rowCount || A.rowCount != A.columnCount)
			{
				throw new Exception("Matrix A must be n*n! A rowCount must be equal B rowCount!");
			}
			MyMatrix X_NEW = new MyMatrix(n, 1);
			MyMatrix X_OLD = new MyMatrix(n, 1);
			double sum = 0.0;
			int iter = 0;
			while (true)
			{
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						if (j != i) sum -= A[i, j] * X_OLD[j, 0];
					}
					if (A[i, i] == 0.0)
					{
						int row = findBiggestValueInColumn(A, i);
						swapRows(A, i, row);
						swapRows(B, i, row);
					}
					X_NEW[i, 0] = (B[i, 0] + sum) / A[i, i];
					sum = 0.0;
				}
				double norm1 = (B - (A * X_NEW)).countNorm();
				double norm2 = B.countNorm();
				if ((norm1 / norm2) < epsilon) { break; }
				for (int i = 0; i < n; i++)
				{
					X_OLD[i, 0] = X_NEW[i, 0];
				}
				iter++;
			}
				Console.WriteLine("Jacobi: {0}", iter);
			return X_NEW;
		}
		public static MyMatrix gaussSeidelIterative(MyMatrix A, MyMatrix B)
		{
			int n = A.rowCount;
			if (A.rowCount != B.rowCount || A.rowCount != A.columnCount)
			{
				throw new Exception("Matrix A must be n*n! A rowCount must be equal B rowCount!");
			}
			MyMatrix X = new MyMatrix(n, 1);
			double sum = 0.0;
			int iter = 0;
			while (true)
			{
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < i; j++)
					{
						sum -= A[i, j] * X[j, 0];
					}
					for (int j = i + 1; j < n; j++)
					{
						sum -= A[i, j] * X[j, 0];
					}
					if (A[i, i] == 0.0)
					{
						int row = findBiggestValueInColumn(A, i);
						swapRows(A, i, row);
						swapRows(B, i, row);
					}
					X[i, 0] = (B[i, 0] + sum) / A[i, i];
					sum = 0.0;
				}
				double norm1 = (B - (A * X)).countNorm();
				double norm2 = B.countNorm();
				if (norm1 / norm2 < epsilon) { break; }
				iter++;
			}
			Console.WriteLine("Seidel: {0}", iter);
			return X;
		}
		public static MyMatrix generateAproxMatrix(int polynomialRank, ResultsFunction data)
		{
			int rows = data.x.Count;
			int columns = 7 + 3 * (polynomialRank - 1);
			MyMatrix matrix = new MyMatrix(rows , columns);
			for (int row = 0; row < rows; row++)
			{
				matrix[row, 0] = data.x[row];
				matrix[row, 1] = data.y[row];
			}
			for (int row = 0; row < rows; row++)
			{
				for(int col = 2; col <= 2 + 2*polynomialRank; col++)
				{
					matrix[row, col] = Math.Pow(data.x[row], col - 2);
				}
			}
			for (int row = 0; row < rows; row++)
			{
				for (int col = 3 + 2*polynomialRank; col < columns; col++)
				{
					matrix[row, col] = data.y[row] * Math.Pow(data.x[row], col - (3 + 2 * polynomialRank));
				}
			}
			return matrix;
		}
		public static MyMatrix getVectorFromAproxMatrix(MyMatrix matrix)
		{
			MyMatrix vector = new MyMatrix(1, matrix.columnCount - 2);
			for(int col = 2; col < matrix.columnCount; col++)
			{
				double sum = 0;
				for(int row = 0; row < matrix.rowCount; row++)
				{
					sum += matrix[row, col];
				}
				vector[0, col - 2] = sum;
			}
			return vector;
		}
		public static MyMatrix createMatrixFromVector(int polynomialRank, MyMatrix vector)
		{
			MyMatrix matrix = new MyMatrix(polynomialRank+1, polynomialRank+2);
			for(int row = 0; row < matrix.rowCount; row++)
			{
				for(int col=0; col < matrix.columnCount; col++)
				{
					matrix[row, col] = vector[0, row + col];
				}
			}
			for(int row = 0; row < matrix.rowCount; row++)
			{
				matrix[row, matrix.columnCount - 1] = vector[0, 3+2*(polynomialRank-1) + row];
			}
			return matrix;
		}


		public void fillDiagonal(int value)
		{
			if (rowCount != columnCount)
			{
				throw new Exception("Wrong matrix size for filling diagonally! Must be n*n!");
			}
			for (int i = 0; i < rowCount; i++)
			{
				matrix[i, i] = value;
			}
		}

		public override string ToString()
		{
			string output = "";
			output += rowCount + " ";
			output += columnCount + "\n";
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < columnCount; j++)
				{

					if (j == columnCount - 1)
					{
						output += matrix[i, j];
					}
					else
						output += matrix[i, j] + " ";
				}
				if (i != rowCount - 1)
				{
					output += '\n';
				}
			}
			return output;
		}

		private static MyMatrix countVariables(MyMatrix AB)
		{
			int n = AB.rowCount;
			MyMatrix X = new MyMatrix(AB.rowCount, 1);
			for (int i = n - 1; i >= 0; i--)
			{
				X[i, 0] = AB[i, n] / AB[i, i];
				for (int k = i - 1; k >= 0; k--)
				{
					AB[k, n] -= AB[k, i] * X[i, 0];
				}
			}
			return X;
		}

		private static int findBiggestValueInColumn(MyMatrix AB, int index)
		{
			double max = AB[0, index];
			int rowIndex = index;
			for (int i = 0; i < AB.rowCount; i++)
			{
				if (Math.Abs(AB[i, index]) > max)
				{
					max = Math.Abs(AB[i, index]);
					rowIndex = i;
				}
			}
			return rowIndex;
		}

		private static void findBiggestValue(MyMatrix AB, int index, List<int> queue) {
            //size :  n x (n+1) / 'AB' Matrix
            double max = AB[index, index];
            int rowIndex = index;
            int columnIndex = index;
            for (int i = index; i < AB.rowCount; i++) {
                for (int j = index; j < AB.columnCount - 1; j++) { //columnCount - 1 , to only look at nxn matrix
                    if (Math.Abs(AB[i, j]) > max) {
                        max = Math.Abs(AB[i, j]);
                        rowIndex = i;
                        columnIndex = j;
                    }
                }
            }
            swapRows(AB, index, rowIndex);
            swapColumns(AB, index, columnIndex, queue);
        }
        private static void swapRows(MyMatrix matrix, int row1, int row2) {
            if (row1 == row2) {
                return;
            }
            for (int i = 0; i < matrix.columnCount; i++) {
                double tmp = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = tmp;
            }
            return;
        }
        private static void swapColumns(MyMatrix matrix, int column1, int column2, List<int> queue) {
            if (column1 == column2) {
                return;
            }
            int tmp = queue[column1];
            queue[column1] = queue[column2];
            queue[column2] = tmp;
            for (int i = 0; i < matrix.rowCount; i++) {
                double Tmp = matrix[i, column1];
                matrix[i, column1] = matrix[i, column2];
                matrix[i, column2] = Tmp;
            }
            return;
        }

        private MyMatrix resultByQueue(MyMatrix vector, List<int> queue) {
            MyMatrix tmp = new MyMatrix(vector.rowCount, 1);
            for (int i = 0; i < vector.rowCount; i++) {
                for (int j = 0; j < vector.columnCount; j++) {
                    tmp[i, j] = vector[i, j];
                }
            }
            for (int i = 0; i < vector.rowCount; i++) {
                vector[queue[i], 0] = tmp[i, 0];
            }
            return vector;
        }

		private double countNorm()
		{
			if (this.columnCount > 1)
				throw new Exception("countNorm() is implemented only for vectors (ATM)");
			double result = 0;
			for (int i = 0; i < this.rowCount; i++)
			{
				result += Math.Pow(this.matrix[i, 0], 2);
			}
			return Math.Sqrt(result);
		}
	}
}
