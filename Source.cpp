#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <ctime>
#include <chrono>
#include <fstream>
#include <iomanip>
#include <Eigen/Dense>
#include <Eigen/SparseLU>
#include <cmath>

// Jêdrzej Dembowski 238242 Informatyka III Tester-Programista gr 1
// Krzysztof Borawski 238152 Informatyka III Tester-Programista gr 1

using namespace std;
using namespace Eigen;

string matrixInput = "matrix.txt";
string vectorInput = "vector.txt";
string vectorNonZeroInput = "vectorNonZero.txt";
string resultOutput = "resultCPP.txt";
int matrixDensity;
double epsilon = 0.0000000001;

VectorXd loadVector(string filename) {
	fstream file(filename, ios_base::in);
	int rows;
	int columns;
	file >> rows;
	file >> columns;
	VectorXd vector(rows);
	for (int i = 0; i < rows; i++) {
		file >> vector(i);
	}
	return vector;
}

MatrixXd loadMatrix(string filename) {
	fstream file(filename, ios_base::in);
	int rows;
	int columns;
	file >> rows;
	file >> columns;
	MatrixXd matrix(rows, columns);
	for (int i = 0; i < rows; i++) {
		for (int j = 0; j < columns; j++) {
			file >> matrix(i, j);
		}
	}
	return matrix;
}

SparseMatrix<double> loadSparseMatrix(string filename, VectorXi vector, int density) {
	fstream file(filename, ios_base::in);
	int rows;
	int columns;
	file >> rows;
	file >> columns;
	double tmp, x, y;
	SparseMatrix<double> matrix(rows, columns);
	matrix.reserve(vector);
	for (int i = 0; i < density; i++) {
		file >> x;
		file >> y;
		file >> tmp;
		matrix.insert(x, y) = tmp;
	}
	return matrix;
}

int countLinesForDensity(string filename) {
	int count = 0;
	string line;
	ifstream file(filename);
	while (std::getline(file, line)) {
		if (line != "") {
			count++;
		}
	}
	count--;
	return count;
}

void saveResults(string filename, int matrixSize, double seidelTime, double sparseLUTime) {
	fstream file;
	file.open(filename, fstream::app);
	//file << "rozmiar;czas gauss seidel eigen;czas gauss sparseLU\n";
	file << matrixSize << ";" << seidelTime << ";" << sparseLUTime << "\n";
	file.close();
}

int findBiggestRowInColumn(SparseMatrix<double> matrix, int column) {
	double max = 0.0;
	int row = 0;
	for (int i = 0; i < matrix.rows(); i++) {
		if (max < matrix.coeff(i, column)) {
			max = matrix.coeff(i, column);
			row = i;
		}
	}
	return row;
}

void swapRowsSparseMatrix(SparseMatrix<double> matrix, int row1, int row2) {
	for (int i = 0; i < matrix.cols(); i++) {
		double tmp = matrix.coeff(row1, i);
		matrix.insert(row1, i) = matrix.coeff(row2, i);
		matrix.insert(row2, i) = tmp;
	}
}

void swapRowsVector(VectorXd matrix, int row1, int row2) {
	double tmp = matrix(row1);
	matrix(row1) = matrix(row2);
	matrix(row2) = tmp;
}

VectorXd gaussSeidel(SparseMatrix<double, RowMajor> matrix, VectorXd vector) {
	//https://eigen.tuxfamily.org/dox/group__TutorialSparse.html
	double norm2 = vector.norm();
	double sum = 0.0;
	VectorXd X(matrix.rows());
	while (true) {
		for (int k = 0; k < matrix.outerSize(); ++k) {
			for (SparseMatrix<double, RowMajor>::InnerIterator it(matrix, k); it; ++it) {
				if (it.col() != it.row()) {
					sum -= it.value()*X(it.col());
				}
				else if (it.value() == 0.0) {
					int row = findBiggestRowInColumn(matrix, it.row());
					swapRowsSparseMatrix(matrix, it.row(), row);
					swapRowsVector(vector, it.row(), row);
				}
			}
			X(k) = (vector(k) + sum) / matrix.coeff(k, k);
			sum = 0.0;
		}
		double norm1 = ((vector - (matrix* X))).norm();
		if ((norm1 / norm2) < epsilon) 
			break;
	}
	return X;
}

void info() {
	srand((unsigned int)time(0));
	cout.precision(std::numeric_limits<double>::max_digits10);
	clock_t start, end;

	matrixDensity = countLinesForDensity(matrixInput);
	cout << "Density : " << matrixDensity << endl;

	printf("\n===================== Gauss SparseLU =====================\n");

	VectorXi vectorNonZero = loadVector(vectorNonZeroInput);
	SparseMatrix<double> matrixSparse = loadSparseMatrix(matrixInput, vectorNonZero, matrixDensity);
	SparseLU<Eigen::SparseMatrix<double> > solver;
	matrixSparse.makeCompressed();
	solver.analyzePattern(matrixSparse);
	solver.factorize(matrixSparse);
	VectorXd vector = loadVector(vectorInput);

	start = clock();
	VectorXd resultSparse = solver.solve(vector);
	end = clock();
	double sparseLUTime = (double(end - start) / CLOCKS_PER_SEC) * 1000;

	cout << "Czas : " << sparseLUTime << endl;
	cout << "Wynik : " << resultSparse(0) << endl;

	printf("\n===================== Gauss Seidel Eigen =====================\n");

	vectorNonZero = loadVector(vectorNonZeroInput);
	SparseMatrix<double, RowMajor> matrixSeidel = loadSparseMatrix(matrixInput, vectorNonZero, matrixDensity);
	matrixSeidel.makeCompressed();
	vector = loadVector(vectorInput);

	start = clock();
	VectorXd resultSeidel = gaussSeidel(matrixSeidel, vector);
	end = clock();
	double seidelTime = (double(end - start) / CLOCKS_PER_SEC) * 1000;

	cout << "Czas : " << seidelTime << endl;
	cout << "Wynik : " << resultSeidel(0) << endl;

	saveResults(resultOutput, vector.rows(), seidelTime, sparseLUTime);
}

int main() {
	info();
	printf("\nKliknij aby zakonczyc: \n");
	cin.get();
	return 0;
}