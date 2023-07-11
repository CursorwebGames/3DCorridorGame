using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine;
using System.Data;

namespace Coder100.Corridors
{
    public class MazeGenerator : MonoBehaviour
    {
        private const int Size = 20;
        private const int RoomSize = 50;
        private const int WallSize = 1;
        private const int WallHeight = 20;

        private readonly Room[,] Grid = new Room[Size, Size];

        private readonly (int, int)[] Dirs = {
            (1, 0),
            (-1, 0),
            (0, 1),
            (0, -1)
        };

        private static Material DebugMaterial;

        [SerializeField]
        private Material _debug_material;

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
                    Grid[row, col] = new(row, col);
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
            DebugMaterial = _debug_material;
            PopulateMaze();
            GenMaze();
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col].Draw();
                }
            }
        }

        private class Room
        {
            public bool Top { get; set; } = true;
            public bool Left { get; set; } = true;
            public bool Bottom { get; set; } = true;
            public bool Right { get; set; } = true;

            private readonly GameObject parent;

            private readonly int row, col;

            public Room(int row, int col)
            {
                parent = new()
                {
                    name = $"Grid[{row},{col}]",
                };

                parent.transform.position = GetPos(row, col);
                this.row = row;
                this.col = col;
            }

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
            public void Draw()
            {
                GameObject light = new("The Light");
                Light lightComp = light.AddComponent<Light>();
                lightComp.range = 20;
                lightComp.intensity = 2;
                lightComp.color = Color.white;
                light.transform.position = new Vector3(RoomSize / 2f, 7, RoomSize / 2f);
                light.transform.SetParent(parent.transform, false);

                if (Top)
                {
                    CreateCube(
                        "Top",
                        position: new Vector3(0, 0, (RoomSize + WallSize) / 2f),
                        scale: new Vector3(WallSize, WallHeight, RoomSize + WallSize)
                    );
                }

                if (Left)
                {
                    CreateCube(
                        "Left",
                        position: new Vector3((RoomSize + WallSize) / 2f, 0, 0),
                        scale: new Vector3(RoomSize + WallSize, WallHeight, WallSize)
                    );
                }

                if (row == Size - 1 && Bottom)
                {
                    CreateCube(
                        "Bottom",
                        position: new Vector3(RoomSize + WallSize, 0, (RoomSize + WallSize) / 2f),
                        scale: new Vector3(WallSize, WallHeight, RoomSize + WallSize)
                    );
                }

                if (col == Size - 1 && Right)
                {
                    CreateCube(
                        "Right",
                        position: new Vector3((RoomSize + WallSize) / 2f, 0, RoomSize + WallSize),
                        scale: new Vector3(RoomSize + WallSize, WallHeight, WallSize)
                    );
                }
            }

            private void CreateCube(string name, Vector3 position, Vector3 scale)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = name;
                cube.transform.position = position;
                cube.transform.localScale = scale;
                cube.transform.SetParent(parent.transform, false);
                cube.GetComponent<MeshRenderer>().material = DebugMaterial;
            }

            private static Vector3 GetPos(int r, int c)
            {
                return new(r * (RoomSize + WallSize), WallHeight / 2, c * (RoomSize + WallSize));
            }
        }
    }
}