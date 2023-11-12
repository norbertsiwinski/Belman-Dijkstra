using System;
using System.IO;

struct Data
{
    public int Distance;
    public int Predecessor;
    public bool Visited;
}

class Program
{
    static int FindMinimum(ref Data[] table)
    {
        int min = -1;
        int minDistance = int.MaxValue;
        for (int i = 0; i < table.Length; i++)
        {
            if (!table[i].Visited && table[i].Distance < minDistance)
            {
                min = i;
                minDistance = table[i].Distance;
            }
        }
        return min;
    }

    static Data[] Dijkstra(int[,] matrix, int start)
    {
        Data[] table = new Data[matrix.GetLength(0)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            table[i].Distance = (i == start) ? 0 : int.MaxValue;
            table[i].Visited = false;
            table[i].Predecessor = -1;
        }
        int u = start;
        while (u != -1)
        {
            table[u].Visited = true;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[u, i] > 0 && table[u].Distance + matrix[u, i] < table[i].Distance)
                {
                    table[i].Distance = table[u].Distance + matrix[u, i];
                    table[i].Predecessor = u;
                }
            }
            u = FindMinimum(ref table);
        }
        return table;
    }

    static void PrintData(int i, Data d)
    {
        Console.Write("{0}\t", i);
        if (!d.Visited)
        {
            Console.Write("unvisited");
        }
        else
        {
            if (d.Predecessor == -1)
                Console.Write("none");
            else Console.Write("{0}", d.Predecessor);
            Console.Write("\t{0}", d.Distance);
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        string fileName = "C:\\Users\\Administrator\\source\\repos\\Belman-Dijkstra\\Dijkstra\\Dijkstraa\\input.txt"; // Change to your file name

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

                Data[] table = Dijkstra(graph, startNode);

                Console.WriteLine("Shortest Paths:");
                for (int target = 0; target < numVertices; target++)
                {
                    Console.Write($"{table[target].Distance} [");
                    int node = target;
                    while (node != -1)
                    {
                        Console.Write(node);
                        if (table[node].Predecessor != -1)
                        {
                            Console.Write(", ");
                        }
                        node = table[node].Predecessor;
                    }
                    Console.WriteLine("]");
                }
            }
        }
        else
        {
            Console.WriteLine("File does not exist.");
        }
    }
}
