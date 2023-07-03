using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public const int Size = 55;
    public const int BlockSize = 5;

    // false = WALL
    // true = NO WALL
    public bool[,] Grid = new bool[Size, Size];

    private (int, int)[] Dirs = {
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    };

    private bool GetCell(int row, int col)
    {
        if (row >= Size || row < 0 || col >= Size || col < 0)
        {
            return true;
        }

        return Grid[row, col];
    }

    private bool SetCells(int row, int col, int dir0, int dir1)
    {
        // there's no wall in other words already visited
        if (GetCell(row + 2 * dir0, col + 2 * dir1)) return false;

        Grid[row + dir0, col + dir1] = true;
        Grid[row + 2 * dir0, col + 2 * dir1] = true;
        return true;
    }

    private void GenMaze()
    {
        Random rnd = new Random();
        HashSet<(int, int)> visited = new();
        Stack<(int, int)> stack = new();

        visited.Add((1, 1));
        stack.Push((1, 1));

        while (stack.Count > 0)
        {
            (int row, int col) = stack.Pop();
            if ((row, col) == (Size - 2, Size - 2))
            {
                continue;
            }

            (int, int)[] randomDirs = Dirs.OrderBy(x => rnd.Next()).ToArray();
            foreach ((int dir0, int dir1) in randomDirs)
            {
                (int, int) next = (row + 2 * dir0, col + 2 * dir1);
                if (!visited.Contains(next) && SetCells(row, col, dir0, dir1))
                {
                    visited.Add(next);
                    stack.Push((row, col));
                    stack.Push(next);
                    break;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenMaze();
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (!Grid[i, j])
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(BlockSize, BlockSize, BlockSize);
                    cube.transform.position = new Vector3(i * BlockSize, 0, j * BlockSize);
                }
            }
        }
    }
}
