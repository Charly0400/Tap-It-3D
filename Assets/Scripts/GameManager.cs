using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab; // Prefab del bloque 
    public int rows = 5; // Número de filas en el nivel
    public int columns = 5; // Número de columnas en el nivel
    public int depth = 5; // Número de capas en el nivel
    public float blockSize = 1.0f; // Tamaño de cada bloque
    public float gapSize = 0.2f; // Espacio entre bloques en todas las direcciones

    private GameObject[,,] blocks; // Matriz para mantener una referencia a los bloques en el nivel actual
    private int currentLevel = 0; // Nivel actual

    void Start()
    {
        GenerateLevel(currentLevel);
    }

    void GenerateLevel(int level)
    {
        // Eliminar bloques existentes si los hay
        if (blocks != null)
        {
            foreach (GameObject block in blocks)
            {
                Destroy(block);
            }
        }

        // Crear una nueva matriz de bloques
        blocks = new GameObject[rows, columns, depth];

        // Generar bloques en filas, columnas y capas
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int dep = 0; dep < depth; dep++)
                {
                    Vector3 position = new Vector3(col * (blockSize + gapSize), row * (blockSize + gapSize), dep * (blockSize + gapSize));
                    GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
                    blocks[row, col, dep] = block;
                }
            }
        }
    }

    void Update()
    {
        // Verificar si todos los bloques han sido eliminados
        if (AllBlocksDestroyed())
        {
            currentLevel++;
            GenerateLevel(currentLevel);
        }
    }

    bool AllBlocksDestroyed()
    {
        // Verificar si cada bloque ha sido destruido
        foreach (GameObject block in blocks)
        {
            if (block != null)
            {
                return false;
            }
        }
        return true;
    }
}
