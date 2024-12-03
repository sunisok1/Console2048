using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Console2048
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPrefab;
        [SerializeField] private Transform textContainer;

        private class Board
        {
            public readonly int m_width;
            public readonly int m_height;

            private readonly int[,] m_data;
            private readonly TextMeshProUGUI[,] m_texts;

            private readonly HashSet<(int, int)> m_emptyGrids = new();

            public Board(int width, int height)
            {
                m_height = height;
                m_width = width;
                m_data = new int[width, height];
                m_texts = new TextMeshProUGUI[width, height];
            }

            public int this[int x, int y]
            {
                get => m_data[x, y];
                set
                {
                    m_data[x, y] = value;
                    m_texts[x, y].text = value.ToString();
                }
            }

            public void Initialize(TextMeshProUGUI prefab, Transform parent)
            {
                for (var x = 0; x < m_width; x++)
                {
                    for (var y = 0; y < m_height; y++)
                    {
                        m_data[x, y] = 0;
                        m_texts[x, y] = Instantiate(prefab, parent);
                    }
                }
            }
        }

        private Board m_board;
        private bool m_gameOver;

        private void Start()
        {
            m_board = new Board(4, 4);
            m_board.Initialize(textPrefab, textContainer);

            AddNewTile();
            AddNewTile();
        }

        private void Update()
        {
            if (m_gameOver) return;
            var keyCode = GetInputValue();
            if (keyCode == KeyCode.None) return;

            switch (keyCode)
            {
                case KeyCode.W:
                    Move(0, -1, 0);
                    break;
                case KeyCode.A:
                    Move(-1, 0, 0);
                    break;
                case KeyCode.S:
                    Move(0, 1, 0);
                    break;
                case KeyCode.D:
                    Move(1, 0, 0);
                    break;
            }

            AddNewTile();

            if (IsGameOver())
            {
                m_gameOver = true;
                Debug.Log("Game Over");
            }
        }

        private KeyCode GetInputValue()
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                return KeyCode.W;
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                return KeyCode.A;
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                return KeyCode.S;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                return KeyCode.D;
            }

            return KeyCode.None;
        }


        private void AddNewTile()
        {
            var row = Random.Range(0, m_board.m_width - 1);
            var col = Random.Range(0, m_board.m_height - 1);
            while (m_board[row, col] != 0)
            {
                row = Random.Range(0, m_board.m_width - 1);
                col = Random.Range(0, m_board.m_height - 1);
            }

            m_board[row, col] = Random.Range(0, 10) > 9 ? 4 : 2; // 90% chance of 2, 10% chance of 4
        }

        private void Move(int rowChange, int colChange, int depth)
        {
            var canMove = false;
            for (var row = 0; row < m_board.m_width; row++)
            {
                for (var col = 0; col < m_board.m_height; col++)
                {
                    if (m_board[row, col] != 0)
                    {
                        var destRow = row + rowChange;
                        var destCol = col + colChange;

                        while (destRow >= 0 && destRow < m_board.m_width && destCol >= 0 && destCol < m_board.m_height)
                        {
                            if (m_board[destRow, destCol] == 0)
                            {
                                m_board[destRow, destCol] = m_board[row, col];
                                m_board[row, col] = 0;
                                canMove = true;
                                destRow += rowChange;
                                destCol += colChange;
                            }
                            else if (m_board[destRow, destCol] == m_board[row, col])
                            {
                                m_board[destRow, destCol] *= 2;
                                m_board[row, col] = 0;
                                canMove = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (!canMove && depth == 0)
            {
                Move(rowChange, colChange, 1);
            }
        }

        private bool IsGameOver()
        {
            for (var i = 0; i < m_board.m_width; i++)
            {
                for (var j = 0; j < m_board.m_height; j++)
                {
                    if (m_board[i, j] == 0)
                    {
                        return false;
                    }
                }
            }

            for (var i = 0; i < m_board.m_width - 1; i++)
            {
                for (var j = 0; j < m_board.m_height - 1; j++)
                {
                    if (m_board[i, j] == m_board[i, j + 1] || m_board[j, i] == m_board[j + 1, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}