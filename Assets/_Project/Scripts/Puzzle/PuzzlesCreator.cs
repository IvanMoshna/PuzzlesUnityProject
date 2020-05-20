using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzlesCreator : MonoBehaviour
{
    public static PuzzleState CreatePuzzle(int height, int width)
    {//    public static List<List<Vector2>> CreatePuzzle(int height, int width)

        int[][] cells;
        cells = new int[width][];
        for (int i = 0; i < width; i++)
        {
            cells[i] = new int[height];
        }

        List<PuzzleData> createdPuzzles=new List<PuzzleData>();

        int counter = 1;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (cells[i][j] != 0) continue;

                cells[i][j] = counter;
                int cellsCreated = 1;
                int cellsCount = Random.Range(2, 5);
                //List<Vector2> blockCells = new List<Vector2>();
                PuzzleData blockCells = new PuzzleData();
                //var blockCells = bCells.puzzleData;
                List<Vector2> blockCellsCanCreateFrom = new List<Vector2>();
                //blockCells.puzzleData.Add(new Vector2(i, j));
                blockCells.puzzleData.Add(new SerializableVector(i, j));
                blockCellsCanCreateFrom.Add(new Vector2(i, j));

                while (cellsCreated <= cellsCount)
                {
                    if (blockCellsCanCreateFrom.Count == 0) break;

                    Vector2 selectedBlock = blockCellsCanCreateFrom[Random.Range(0, blockCellsCanCreateFrom.Count)];
                    int x = (int) selectedBlock.x;
                    int y = (int) selectedBlock.y;

                    List<Vector2> variants = new List<Vector2>();
                    int minX = x;
                    int maxX = x;
                    int minY = y;
                    int maxY = y;
                    foreach (var variant in blockCells.puzzleData)
                    {
                        if (variant.x < minX) minX = (int) variant.x;
                        if (variant.x > maxX) maxX = (int) variant.x;
                        if (variant.y < minY) minY = (int) variant.y;
                        if (variant.y > maxY) maxY = (int) variant.y;
                    }

                    if (x + 1 < width && cells[x + 1][y] == 0 && x + 1 < minX + 3)
                    {
                        variants.Add(new Vector2(x + 1, y));
                    }

                    if (x - 1 >= 0 && cells[x - 1][y] == 0 && x - 1 > maxX - 3)
                    {
                        variants.Add(new Vector2(x - 1, y));
                    }

                    if (y + 1 < height && cells[x][y + 1] == 0 && y + 1 < minY + 3)
                    {
                        variants.Add(new Vector2(x, y + 1));
                    }

                    if (y - 1 >= 0 && cells[x][y - 1] == 0 && y - 1 > maxY - 3)
                    {
                        variants.Add(new Vector2(x, y - 1));
                    }

                    if (variants.Count == 0)
                    {
                        blockCellsCanCreateFrom.Remove(selectedBlock);
                        continue;
                    }

                    Vector2 selectedVariant = variants[Random.Range(0, variants.Count)];
                    //blockCells.puzzleData.Add(selectedVariant);
                    blockCells.puzzleData.Add(new SerializableVector(selectedVariant.x, selectedVariant.y));
                    blockCellsCanCreateFrom.Add(selectedVariant);
                    cells[(int) selectedVariant.x][(int) selectedVariant.y] = counter;
                    cellsCreated++;
                }
                
                //createdPuzzles.Add(blockCells);
                createdPuzzles.Add(blockCells);
                

                counter++;
                //Debug.Log("COUNTER = " + counter);
            }
        }

        PuzzleState puzzleState = new PuzzleState(createdPuzzles);
        return puzzleState;
        
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (cells[i][j] < 10)
                    stringBuilder.Append("  " + cells[i][j] + " ");
                else
                    stringBuilder.Append(" " + cells[i][j] + " ");
            }

            stringBuilder.Append("\n");
        }

        Debug.Log(stringBuilder.ToString());
    }
}