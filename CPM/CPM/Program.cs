int processTime = 0;

var file = new FileInfo("C:\\Users\\siwinskn\\source\\repos\\CPM\\CPM\\Files\\data.txt");
if (!file.Exists) throw new FileNotFoundException();

var lines = File.ReadAllLines(file.FullName);
var scanner = new Queue<string>(lines.SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)));

int N = int.Parse(scanner.Dequeue());
int M = int.Parse(scanner.Dequeue());

int[] durations = new int[N];
for (int i = 0; i < N; i++)
{
    durations[i] = int.Parse(scanner.Dequeue());
}

int[,] dependencies = new int[M, 2];
for (int i = 0; i < M; i++)
{
    dependencies[i, 0] = int.Parse(scanner.Dequeue());
    dependencies[i, 1] = int.Parse(scanner.Dequeue());
}

var tasks = CreateTasks(durations);
SetPredecessorsAndSuccessors(dependencies, tasks);

var startTime = DateTime.Now;

FindStartingTasks(tasks);

foreach (var task in tasks)
{
    processTime = Math.Max(processTime, task.EarlyFinish);
}

FindEndingTasks(processTime, tasks);

Console.WriteLine("process time:");
Console.WriteLine(processTime);

//Console.WriteLine("earlyStart earlyFinish lateStart lateFinish:");
//foreach (var task in tasks)
//{
//    Console.WriteLine($"{task.EarlyStart} {task.EarlyFinish} {task.LateStart} {task.LateFinish}");
//}

var criticalPath = ComputeCriticalPath(tasks);

var endTime = DateTime.Now;
var timeTaken = (endTime - startTime).TotalSeconds;

Console.WriteLine("critical path:");
foreach (var task in criticalPath)
{
    Console.WriteLine($"{task.Index + 1} {task.EarlyStart} {task.EarlyFinish}");
}

Console.WriteLine("-----------------------------------------");
Console.WriteLine($"Time taken: {timeTaken} seconds");

Task[] CreateTasks(int[] durations)
{
    var tasks = new Task[durations.Length];
    for (int i = 0; i < durations.Length; i++)
    {
        tasks[i] = new Task(i, durations[i]);
    }
    return tasks;
}

void SetPredecessorsAndSuccessors(int[,] dependencies, Task[] tasks)
{
    for (int i = 0; i < dependencies.GetLength(0); i++)
    {
        var predecessor = tasks[dependencies[i, 0] - 1];
        var successor = tasks[dependencies[i, 1] - 1];
        predecessor.Successors.Add(successor);
        successor.Predecessors.Add(predecessor);
    }
}

void FindStartingTasks(Task[] tasks)
{
    foreach (var task in tasks)
    {
        if (!task.Predecessors.Any())
        {
            ForwardPass(task);
        }
    }
}

void ForwardPass(Task task)
{
    task.EarlyFinish = task.EarlyStart + task.Duration;
    foreach (var successor in task.Successors)
    {
        successor.EarlyStart = Math.Max(successor.EarlyStart, task.EarlyFinish);
        ForwardPass(successor);
    }
}

void FindEndingTasks(int projectTime, Task[] tasks)
{
    foreach (var task in tasks)
    {
        if (!task.Successors.Any())
        {
            task.LateFinish = projectTime;
            BackwardPass(task);
        }
    }
}

void BackwardPass(Task task)
{
    task.LateStart = task.LateFinish - task.Duration;
    foreach (var predecessor in task.Predecessors)
    {
        predecessor.LateFinish = Math.Min(predecessor.LateFinish, task.LateStart);
        BackwardPass(predecessor);
    }
}

List<Task> ComputeCriticalPath(Task[] tasks)
{
    var criticalPath = tasks.Where(task => task.EarlyStart == task.LateStart).ToList();
    criticalPath.Sort((t1, t2) => t1.EarlyStart.CompareTo(t2.EarlyStart));
    return criticalPath;
}

public class Task
{
    public int Index { get; }
    public int Duration { get; }
    public int EarlyStart { get; set; } = 0;
    public int EarlyFinish { get; set; } = 0;
    public int LateStart { get; set; } = int.MaxValue;
    public int LateFinish { get; set; } = int.MaxValue;
    public List<Task> Successors { get; } = new();
    public List<Task> Predecessors { get; } = new();

    public Task(int index, int duration)
    {
        Index = index;
        Duration = duration;
    }
}