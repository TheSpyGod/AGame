﻿using Adventure;
using AGConsole.Engine.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace AGConsole.Engine.Render
{
    internal class OpenWrld
    {
        public char[,] map { get; private set; }
        private char[,] CombatMap;
        private char[,] tempMap;
        private Vector2 TempPlayerLoc;
        private int _cellSize;
        private int _numCellsWidth;
        private int _numCellsHeight;
        private ShapeDrawer _shapeDrawer;
        private Control _control;
        private bool InitCombat = false;
        private ActorModel.Actor player;
        private ActorModel.Actor enemy;
        private const int DefaultCellSize = 15;
        private Random rand = new Random();
        private List<Vector2> directions = new List<Vector2>
        {
            new Vector2(-1, 0), // up
            new Vector2(1, 0),  // down
            new Vector2(0, -1), // left
            new Vector2(0, 1)   // right
        };

        public OpenWrld(Control control)
        {
            _control = control;
            _shapeDrawer = new ShapeDrawer(control);
            _control.Resize += OnResize;  // Handle resizing
            UpdateMapDimensions();
            GenerateMap();
            PrintScene();
        }

        private void OnResize(object sender, EventArgs e)
        {
            UpdateMapDimensions();
            UpdateDrawing();
        }

        private void UpdateMapDimensions()
        {
            _numCellsWidth = _control.ClientSize.Width / DefaultCellSize;
            _numCellsHeight = _control.ClientSize.Height / DefaultCellSize;
            _cellSize = Math.Min(_control.ClientSize.Width / _numCellsWidth, _control.ClientSize.Height / _numCellsHeight); // Recalculate cell size
        }

        private char[,] GenerateMap()
        {
            if (map == null)
            {
                map = new char[_numCellsWidth, _numCellsHeight];
            }
            for (int i = 0; i < _numCellsWidth; i++)
            {
                for (int j = 0; j < _numCellsHeight; j++)
                {
                    if (rand.Next(0, 10) < 2)
                    {
                        map[i, j] = '*';
                    }
                    else
                    {
                        map[i, j] = ' ';
                    }
                }
            }
            return map;
        }

        public void UpdateDrawing()
        {
            var shapes = new List<(PointF[], Brush)>();
            _shapeDrawer.ClearShapes();
            // Draw map
            for (int i = 0; i < _numCellsWidth; i++)
            {
                for (int j = 0; j < _numCellsHeight; j++)
                {
                    if (map[i, j] == '*')
                    {
                        AddSquareShape(shapes, i, j, Brushes.Black);
                    }
                }
            }

            AddSquareShape(shapes, (int)player.Location.X, (int)player.Location.Y, Brushes.WhiteSmoke); // Brush for player
            if (enemy.IsAlive) AddSquareShape(shapes, (int)enemy.Location.X, (int)enemy.Location.Y, Brushes.Red);

            if (player.Location.X == enemy.Location.X && player.Location.Y == enemy.Location.Y)
            {
                //enemy.IsAlive = false; // If the enemy is hit
                TempPlayerLoc = player.Location;
                player.Location = new Vector2(0, 0);
                InitCombat = true;
                tempMap = map;
                map = CombatMap;
            }

            foreach (var (vertices, brush) in shapes)
            {
                _shapeDrawer.AddShape(vertices, brush);
            }
        }

        private void AddSquareShape(List<(PointF[], Brush)> shapes, int x, int y, Brush brush)
        {
            var topLeft = new PointF(x * _cellSize, y * _cellSize);
            var vertices = new[]
            {
                topLeft,
                new PointF(topLeft.X + _cellSize, topLeft.Y),
                new PointF(topLeft.X + _cellSize, topLeft.Y + _cellSize),
                new PointF(topLeft.X, topLeft.Y + _cellSize)
            };
            shapes.Add((vertices, brush));
        }

        public bool HandlePlayerMovement(Keys key)
        {
            int newRow = (int)player.Location.X;
            int newCol = (int)player.Location.Y;
            if (!InitCombat)
            {
                switch (key)
                {
                    case Keys.Left:
                        if (newRow > 0) newRow -= 1;
                        break;
                    case Keys.Right:
                        if (newRow < _numCellsWidth - 1) newRow += 1;
                        break;
                    case Keys.Up:
                        if (newCol > 0) newCol -= 1;
                        break;
                    case Keys.Down:
                        if (newCol < _numCellsHeight - 1) newCol += 1;
                        break;
                    case Keys.Escape:
                        return true;
                }
            }
            if (!InitCombat && newRow >= 0 && newRow < _numCellsWidth && newCol >= 0 && newCol < _numCellsHeight && map[newRow, newCol] != '*')
            {
                player.Location = new Vector2(newRow, newCol);
            }
            return false;
        }

        public void HandleEnemyTurn()
        {
            var nextStep = EnemyPathFinding(enemy.Location, player.Location);
            float distance = Math.Abs(enemy.Location.X - player.Location.X) + Math.Abs(enemy.Location.Y - player.Location.Y);
            var newDirection = enemy.Location + directions[rand.Next(0, directions.Count)];

            if (nextStep != null && distance < 5 && nextStep != player.Location)
            {
                enemy.Location = nextStep.Value;
            }
            else if
            (!InitCombat && newDirection.X >= 0 && newDirection.X < _numCellsWidth &&
             newDirection.Y >= 0 && newDirection.Y < _numCellsHeight &&
             map[(int)newDirection.X, (int)newDirection.Y] != '*')
            {
                enemy.Location = newDirection;
            }

        }

        private Vector2? EnemyPathFinding(Vector2 start, Vector2 target)
        {
            var queue = new Queue<(Vector2 Position, Vector2? Parent)>();
            var visited = new bool[_numCellsWidth, _numCellsHeight];
            var parentMap = new Dictionary<Vector2, Vector2>();
            parentMap[start] = start;
            queue.Enqueue((start, null));
            visited[(int)start.X, (int)start.Y] = true;

            while (queue.Count > 0)
            {
                var (current, parent) = queue.Dequeue();

                if (current == target)
                {
                    var step = current;
                    while (parentMap[step] != start)
                    {
                        step = parentMap[step];
                    }
                    return step;
                }

                foreach (var direction in directions)
                {
                    var next = current + direction;
                    int nextX = (int)next.X;
                    int nextY = (int)next.Y;

                    if (nextX >= 0 && nextX < _numCellsWidth && nextY >= 0 && nextY < _numCellsHeight &&
                        !visited[nextX, nextY] && map[nextX, nextY] != '*')
                    {
                        queue.Enqueue((next, current));
                        visited[nextX, nextY] = true;
                        parentMap[next] = current;
                    }
                }
            }
            return null;
        }

        public void SetActors(ActorModel.Actor player, ActorModel.Actor enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }
        private void PrintScene()
        {
            if (CombatMap == null)
            {
                CombatMap = new char[_numCellsWidth, _numCellsHeight];
            }
            for (int i = 0; i < _numCellsWidth; i++)
            {
                for (int j = 0; j < _numCellsHeight; j++)
                {
                    CombatMap[i, j] = '*';
                }
            }
        }
    }
}
