
    string fileName = "C:\\Users\\siwinskn\\source\\repos\\Dijkstra\\Dijkstra\\input.txt"; // Zmień na nazwę swojego pliku

    if (File.Exists(fileName))
    {
        using (StreamReader reader = File.OpenText(fileName))
        {
            int numVertices = int.Parse(reader.ReadLine());

            int[,] graph = new int[numVertices, numVertices];

            for (int i = 0; i < numVertices; i++)
            {
            string[] input = reader.ReadLine().Split(new[] {"  "}, StringSplitOptions.RemoveEmptyEntries); 

            for (int j = 0; j < numVertices; j++)
                {
                    graph[i, j] = int.Parse(input[j]);
                }
            }

            int startNode = 0;

            int[] distance = new int[numVertices];
            int[] predecessor = new int[numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                distance[i] = int.MaxValue;
                predecessor[i] = -1;
            }

            distance[startNode] = 0;

            for (int i = 0; i < numVertices - 1; i++)
            {
                for (int u = 0; u < numVertices; u++)
                {
                    for (int v = 0; v < numVertices; v++)
                    {
                        int weight = graph[u, v];
                        if (weight != 0 && distance[u] != int.MaxValue && distance[u] + weight < distance[v])
                        {
                            distance[v] = distance[u] + weight;
                            predecessor[v] = u;
                        }
                    }
                }
            }

            Console.WriteLine("Najkrótsze Ścieżki:");
            for (int target = 0; target < numVertices; target++)
            {
                Console.Write($"{distance[target]} [");
                int node = target;
                while (node != -1)
                {
                    Console.Write(node);
                    if (predecessor[node] != -1)
                    {
                        Console.Write(", ");
                    }
                    node = predecessor[node];
                }
                Console.WriteLine("]");
            }
        }
    }
    else
    {
        Console.WriteLine("Plik nie istnieje.");
    }





