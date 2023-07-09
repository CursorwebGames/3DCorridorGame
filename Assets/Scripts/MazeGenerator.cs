using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private const int Size = 20;
    private const int RoomSize = 50;
    private const int WallSize = 1;
    private const int WallHeight = 10;

    private readonly Room[,] Grid = new Room[Size, Size];

    private readonly (int, int)[] Dirs = {
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1)
    };

    /// <summary>
    /// <para>false = Wall</para>
    /// <para>true = No Wall</para>
    /// </summary>
    private Room GetRoom(int row, int col)
    {
        if (row >= Size || row < 0 || col >= Size || col < 0)
        {
            return null;
        }

        return Grid[row, col];
    }

    private bool SetRoom(int row, int col, int dr, int dc)
    {
        Room nextRoom = GetRoom(row + dr, col + dc);
        if (nextRoom == null)
        {
            return false;
        }

        Room currRoom = Grid[row, col];

        switch ((dr, dc))
        {
            case (1, 0):
                currRoom.Bottom = nextRoom.Top = false;
                break;

            case (-1, 0):
                currRoom.Top = nextRoom.Bottom = false;
                break;

            case (0, 1):
                currRoom.Right = nextRoom.Left = false;
                break;

            case (0, -1):
                currRoom.Left = nextRoom.Right = false;
                break;
        }

        return true;
    }

    private void PopulateMaze()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                Grid[row, col] = new();
            }
        }
    }

    private void GenMaze()
    {
        Random rnd = new();
        HashSet<(int, int)> visited = new();
        Stack<(int, int)> stack = new();

        visited.Add((0, 0));
        stack.Push((0, 0));

        while (stack.Count > 0)
        {
            (int row, int col) = stack.Pop();
            if ((row, col) == (Size - 1, Size - 1))
            {
                continue;
            }

            (int, int)[] randomDirs = Dirs.OrderBy(x => rnd.Next()).ToArray();
            foreach ((int dr, int dc) in randomDirs)
            {
                (int, int) next = (row + dr, col + dc);
                if (!visited.Contains(next) && SetRoom(row, col, dr, dc))
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
        PopulateMaze();
        GenMaze();
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                Grid[row, col].Draw(row, col);
            }
        }
    }

    private class Room
    {
        public bool Top { get; set; } = true;
        public bool Left { get; set; } = true;
        public bool Bottom { get; set; } = true;
        public bool Right { get; set; } = true;

        /// <summary>
        /// <para>We draw like this:</para>
        /// <code>
        ///  _
        /// |
        /// </code>
        /// <para>The direction is:</para>
        /// <code>
        ///  --> z
        /// |
        /// V x
        /// </code>
        /// </summary>
        public void Draw(int row, int col)
        {
            GameObject parent = new()
            {
                name = $"Grid[{row},{col}]",
            };

            parent.transform.position = GetPos(row, col);

            if (Top)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Top";
                cube.transform.position = new Vector3(0, 0, (RoomSize + WallSize) / 2f);
                cube.transform.localScale = new Vector3(WallSize, WallHeight, RoomSize + WallSize);
                cube.transform.SetParent(parent.transform, false);
            }

            if (Left)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Left";
                cube.transform.position = new Vector3((RoomSize + WallSize) / 2f, 0, 0);
                cube.transform.localScale = new Vector3(RoomSize + WallSize, WallHeight, WallSize);
                cube.transform.SetParent(parent.transform, false);
            }

            if (row == Size - 1 && Bottom)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Bottom";
                cube.transform.position = new Vector3(RoomSize + WallSize, 0, (RoomSize + WallSize) / 2f);
                cube.transform.localScale = new Vector3(WallSize, WallHeight, RoomSize + WallSize);
                cube.transform.SetParent(parent.transform, false);
            }

            if (col == Size - 1 && Right)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Right";
                cube.transform.position = new Vector3((RoomSize + WallSize) / 2f, 0, RoomSize + WallSize);
                cube.transform.localScale = new Vector3(RoomSize + WallSize, WallHeight, WallSize);
                cube.transform.SetParent(parent.transform, false);
            }
        }

        private Vector3 GetPos(int r, int c)
        {
            return new(r * (RoomSize + WallSize), 0, c * (RoomSize + WallSize));
        }
    }
}
