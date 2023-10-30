string fileName = "C:\\Users\\siwinskn\\source\\repos\\Dijkstra\\Dijkstra\\input.txt"; // Zmień na nazwę swojego pliku

if (File.Exists(fileName))
{
    using (StreamReader reader = File.OpenText(fileName))
    {
        int numVertices = int.Parse(reader.ReadLine());

        int[,] graph = new int[numVertices, numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            string[] input = reader.ReadLine().Split(new[] { "  " }, StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < numVertices; j++)
            {
                graph[i, j] = int.Parse(input[j]);
            }
        }

        int startNode = 0;

        DijkstraAlgorithm(graph, startNode);
    }
}
else
{
    Console.WriteLine("Plik nie istnieje.");
}

void DijkstraAlgorithm(int[,] graph, int startNode)
{
    int numVertices = graph.GetLength(0);
    int[] distance = new int[numVertices];
    bool[] shortestPathTreeSet = new bool[numVertices];

    for (int i = 0; i < numVertices; i++)
    {
        distance[i] = int.MaxValue;
        shortestPathTreeSet[i] = false;
    }

    distance[startNode] = 0;

    for (int count = 0; count < numVertices - 1; count++)
    {
        int u = MinimumDistance(distance, shortestPathTreeSet, numVertices);
        shortestPathTreeSet[u] = true;

        for (int v = 0; v < numVertices; v++)
        {
            if (!shortestPathTreeSet[v] && graph[u, v] != 0 &&
                distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
            {
                distance[v] = distance[u] + graph[u, v];
            }
        }
    }

    Console.WriteLine("Najkrótsze Ścieżki:");
    for (int i = 0; i < numVertices; i++)
    {
        Console.WriteLine($"{i} [{distance[i]}]");
    }
}

int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int numVertices)
{
    int minDistance = int.MaxValue;
    int minIndex = -1;

    for (int v = 0; v < numVertices; v++)
    {
        if (!shortestPathTreeSet[v] && distance[v] <= minDistance)
        {
            minDistance = distance[v];
            minIndex = v;
        }
    }

    return minIndex;
}
