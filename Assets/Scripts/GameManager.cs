using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Direction { Arriba, Abajo, Izquierda, Derecha, Delante, Atras }

public class GameManager : MonoBehaviour
{
    public Timer timer; // Referencia al script Timer
    public List<GameObject> blockPrefabs; // Lista de prefabs con direcciones rotadas
    public int rows = 5; // Número de filas en el nivel
    public int columns = 5; // Número de columnas en el nivel
    public int depth = 5; // Número de capas en el nivel
    public float blockSize = 1.0f; // Tamaño de cada bloque
    public float gapSize = 0.2f; // Espacio entre bloques en todas las direcciones

    private GameObject[,,] blocks; // Matriz para mantener una referencia a los bloques en el nivel actual
    public int currentLevel = 0; // Nivel actual

    void Start()
    {
        GenerateLevel(currentLevel);
    }

    void Update()
    {
        // Verificar si todos los bloques han sido eliminados
        if (AllBlocksDestroyed())
        {
            // Si todos los bloques han sido destruidos, reinicia el temporizador y genera un nuevo nivel
            timer.ResetTimer();
            currentLevel++;
            GenerateLevel(currentLevel);
        }
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

        // Generar dimensiones aleatorias para el siguiente nivel
        rows = Random.Range(2, 4); 
        columns = Random.Range(2, 4); 
        depth = Random.Range(2, 4); 

        // Crear una nueva matriz de bloques
        blocks = new GameObject[rows, columns, depth];

        // Generar bloques en filas, columnas y capas
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int dep = 0; dep < depth; dep++)
                {
                    // Seleccionar un prefab al azar de la lista
                    GameObject selectedPrefab = blockPrefabs[Random.Range(0, blockPrefabs.Count)];

                    // Instanciar el bloque
                    GameObject block = Instantiate(selectedPrefab, GetBlockPosition(row, col, dep), Quaternion.identity);

                    // Obtener el componente CubosInteractuables del bloque
                    CubosInteractuables cubosInteractuables = block.GetComponent<CubosInteractuables>();

                    // Asignar una dirección aleatoria no opuesta al componente CubosInteractuables
                    Direction randomDirection = GetRandomNonOppositeDirection(row, col, dep);
                    cubosInteractuables.blockDirection = randomDirection;

                    // Ajustar la rotación del modelo del cubo según la dirección
                    Quaternion rotation = Quaternion.identity;
                    switch (randomDirection)
                    {
                        case Direction.Arriba:
                            rotation = Quaternion.identity;
                            break;
                        case Direction.Abajo:
                            rotation = Quaternion.Euler(180f, 0f, 0f);
                            break;
                        case Direction.Izquierda:
                            rotation = Quaternion.Euler(0f, 0f, 90f);
                            break;
                        case Direction.Derecha:
                            rotation = Quaternion.Euler(0f, 0f, -90f);
                            break;
                        case Direction.Delante:
                            rotation = Quaternion.Euler(90f, 0f, 0f);
                            break;
                        case Direction.Atras:
                            rotation = Quaternion.Euler(-90f, 0f, 0f);
                            break;
                    }
                    block.transform.rotation = rotation;

                    // Asignar el bloque a la matriz
                    blocks[row, col, dep] = block;
                }
            }
        }

        // Validar el nivel para asegurarse de que haya una solución posible
        if (!ValidateLevel())
        {
            Debug.LogWarning("El nivel generado no tiene una solución posible. Generando nuevo nivel...");
            GenerateLevel(level); // Intentar generar un nuevo nivel si el actual no es válido
        }
    }

    Direction GetRandomNonOppositeDirection(int row, int col, int dep)
    {
        List<Direction> possibleDirections = new List<Direction>(Enum.GetValues(typeof(Direction)) as Direction[]);
        Direction oppositeDirection = Direction.Arriba; // Por defecto

        if (dep > 0)
        {
            oppositeDirection = GetOppositeDirection(blocks[row, col, dep - 1].GetComponent<CubosInteractuables>().blockDirection);
        }

        // Remover la dirección opuesta de las posibles direcciones
        possibleDirections.Remove(oppositeDirection);

        // Devolver una dirección aleatoria de las posibles direcciones restantes
        return possibleDirections[Random.Range(0, possibleDirections.Count)];
    }

    Direction GetOppositeDirection(Direction direction)
    {
        // Devuelve la dirección opuesta
        switch (direction)
        {
            case Direction.Arriba: return Direction.Abajo;
            case Direction.Abajo: return Direction.Arriba;
            case Direction.Izquierda: return Direction.Derecha;
            case Direction.Derecha: return Direction.Izquierda;
            case Direction.Delante: return Direction.Atras;
            case Direction.Atras: return Direction.Delante;
            default: return Direction.Arriba; // En caso de error, devuelve Arriba
        }
    }

    Vector3 GetBlockPosition(int row, int col, int dep)
    {
        // Calcular la posición del bloque en función de la fila, columna y capa
        float posX = col * (blockSize + gapSize);
        float posY = row * (blockSize + gapSize);
        float posZ = dep * (blockSize + gapSize);
        return new Vector3(posX, posY, posZ);
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
    bool ValidateLevel()
    {
        // Lista de bloques que se pueden eliminar
        List<GameObject> removableBlocks = new List<GameObject>();

        // Llenar la lista con todos los bloques iniciales
        foreach (GameObject block in blocks)
        {
            if (block != null)
            {
                removableBlocks.Add(block);
            }
        }

        // Simular el proceso de eliminación de bloques
        bool changesMade = true;
        while (changesMade)
        {
            changesMade = false;
            List<GameObject> blocksToRemove = new List<GameObject>();

            // Iterar sobre cada bloque en la lista de bloques que se pueden eliminar
            foreach (GameObject block in removableBlocks)
            {
                // Comprobar si hay bloques adyacentes que se puedan eliminar también
                Collider[] colliders = Physics.OverlapBox(block.transform.position, Vector3.one * 1.1f, Quaternion.identity);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != block && removableBlocks.Contains(collider.gameObject))
                    {
                        blocksToRemove.Add(collider.gameObject);
                        changesMade = true;
                    }
                }
            }

            // Eliminar los bloques que se pueden eliminar
            foreach (GameObject blockToRemove in blocksToRemove)
            {
                removableBlocks.Remove(blockToRemove);
            }
        }

        // Verificar si todos los bloques se han eliminado
        return removableBlocks.Count == 0;
    }
    // Método llamado cuando el temporizador llega a cero


}
