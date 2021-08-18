using System;

public class SudokuSolver
{
    private int _sizeOfBoard = 9;

    /// <summary>
    /// Checks the sudoku grid recursively if has any solution.
    /// </summary>
    /// <param name="sudokuGridToTest"></param>
    /// <param name="slotIndex"></param>
    /// <returns>Returns true if there's any solution to solve the sudoku grid.</returns>
    public bool HasAnySolution(int[,] sudokuGridToTest, int slotIndex = 0)
    {
        // Checking if the board completely filled
        if (slotIndex == 81)
        {
            return true;
        }
        
        int[,] copiedSudokuGrid = (int[,]) sudokuGridToTest.Clone();

        int row = (int) Math.Floor((double) (slotIndex / _sizeOfBoard));
        int column = slotIndex % _sizeOfBoard;

        // Checking the slot has already filled
        if (copiedSudokuGrid[row, column] != 0)
        {
            return HasAnySolution(copiedSudokuGrid, slotIndex + 1);
        }

        // Testing the numbers(1-9) if it's lead to any solution
        for (int numberToTry = 1; numberToTry <= 9; numberToTry++)
        {
            // If there's no conflict (row-column-region) assign the number
            if (IsValid(copiedSudokuGrid, row, column, numberToTry))
            {
                copiedSudokuGrid[row, column] = numberToTry;

                // Check next slot to see if it's okay
                // If not reset the slot
                if (!HasAnySolution(copiedSudokuGrid, slotIndex + 1))
                {
                    copiedSudokuGrid[row, column] = 0;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks the sudoku grid's every cell if it has only one valid solution.
    /// </summary>
    /// <param name="sudokuGridToTest"></param>
    /// <returns>Returns true if there's only one solution.</returns>
    public bool HasUniqueSolution(int[,] sudokuGridToTest)
    {
        int[,] copiedSudokuGrid = (int[,]) sudokuGridToTest.Clone();
        
        // Controlling every cell
        for (int row = 0; row < _sizeOfBoard; row++)
        {
            for (int column = 0; column < _sizeOfBoard ; column++)
            {
                // Checking if the cell is empty
                if (copiedSudokuGrid[row, column] == 0)
                {
                    int solutionCount = 0;

                    // Checking every number(1-9) for if it's valid for the cell
                    // If the number is valid we increase the solution count
                    for (int numberToTry = 1; numberToTry <= 9; numberToTry++)
                    {
                        if (IsValid(copiedSudokuGrid, row, column, numberToTry))
                        {
                            solutionCount++;
                            copiedSudokuGrid[row, column] = numberToTry;
                        }
                    }

                    // If there's more than one valid number for the cell we return false
                    if (solutionCount != 1)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Checks every row-column-region to see if it's okay to place the number according to sudoku constraints.
    /// </summary>
    /// <param name="sudokuGrid"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="numberToTry"></param>
    /// <returns>Returns true if it's okay to place the number.</returns>
    public bool IsValid(int[,] sudokuGrid, int row, int column, int numberToTry)
    {
        for (int i = 0; i < 9; i++)
        {
            // Checking every column
            if (sudokuGrid[row, i] == numberToTry)
            {
                return false;
            }

            // Checking every row
            if (sudokuGrid[i, column] == numberToTry)
            {
                return false;
            }
        }

        // Calculating the region's first and last indexes
        int regionStartOfRowIndex = row - (row % 3);
        int regionStartOfColumnIndex = column - (column % 3);

        int regionEndOfRowIndex = regionStartOfRowIndex + 2;
        int regionEndOfColumnIndex = regionStartOfColumnIndex + 2;

        // Checking the region
        for (int x = regionStartOfRowIndex; x <= regionEndOfRowIndex; x++)
        {
            for (int y = regionStartOfColumnIndex; y <= regionEndOfColumnIndex; y++)
            {
                if (sudokuGrid[x, y] == numberToTry)
                {
                    return false;
                }
            }
        }

        return true;
    }
}