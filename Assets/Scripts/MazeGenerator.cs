using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public const int Size = 15;
    public const int BlockSize = 50;
    public const int WallSize = 1;
    public const int WallHeight = 10;

    // false = WALL
    // true = NO WALL
    public bool[,] Grid = new bool[Size, Size];

    private (int, int)[] Dirs = {
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    };

    /// <summary>
    /// <para>false = Wall</para>
    /// <para>true = No Wall</para>
    /// </summary>
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
    private void Start()
    {
        GenMaze();
        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // cube.transform.position = getPos(r, c);
                // cube.transform.localScale = new Vector3();
                if (!GetCell(r, c))
                {
                    Vector3 pos = getPos(r, c);
                    if (!GetCell(r + 1, c)) // _
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.localScale = new Vector3(BlockSize + WallSize, WallHeight, WallSize);
                        cube.transform.position = new Vector3((BlockSize + WallSize) / 2f, 0, -(BlockSize + WallSize)) + pos;
                    }

                    if (!GetCell(r - 1, c)) // -
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.localScale = new Vector3(BlockSize + WallSize, WallHeight, WallSize);
                        cube.transform.position = new Vector3((BlockSize + WallSize) / 2f, 0, 0) + pos;
                    }

                    if (!GetCell(r + 1, c)) // |
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.localScale = new Vector3(WallSize, WallHeight, BlockSize + WallSize);
                        cube.transform.position = new Vector3(0, 0, -(BlockSize + WallSize) / 2f) + pos;
                    }

                    if (!GetCell(r + 1, c)) // -|
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.localScale = new Vector3(WallSize, WallHeight, BlockSize + WallSize);
                        cube.transform.position = new Vector3(BlockSize + WallSize, 0, -(BlockSize + WallSize) / 2f) + pos;
                    }
                }
            }
        }

        /*
        // _
        GameObject cube;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new(BlockSize + WallSize, WallHeight, WallSize);
        cube.transform.position = new((BlockSize + WallSize) / 2f, 0, -(BlockSize + WallSize));

        // -
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new(BlockSize + WallSize, WallHeight, WallSize);
        cube.transform.position = new((BlockSize + WallSize) / 2f, 0, 0);

        // |
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new(WallSize, WallHeight, BlockSize + WallSize);
        cube.transform.position = new(0, 0, -(BlockSize + WallSize) / 2f);

        // -|
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new(WallSize, WallHeight, BlockSize + WallSize);
        cube.transform.position = new(BlockSize + WallSize, 0, -(BlockSize + WallSize) / 2f);
        */

        // GenMaze();
        // for (int i = 0; i < Size; i++)
        // {
        //     for (int j = 0; j < Size; j++)
        //     {
        //         if (!Grid[i, j])
        //         {
        //             GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //             cube.transform.localScale = new Vector3(BlockSize, BlockSize, BlockSize);
        //             cube.transform.position = new Vector3(i * BlockSize, 0, j * BlockSize);
        //         }
        //     }
        // }
    }

    private Vector3 getPos(int r, int c)
    {
        return new(r * (BlockSize + WallSize), 0, -c * (BlockSize + WallSize));
    }
}
