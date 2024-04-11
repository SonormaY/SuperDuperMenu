namespace SuperDuperMenuLib;

public class SuperDuperMenu
{
    private readonly Dictionary<string, Action> _tasks = new();
    public void AddTask(string taskName, Action task)
    {
        if (_tasks.ContainsKey(taskName))
        {
            return;
        }

        taskName = taskName.First().ToString().ToUpper() + taskName.Substring(1);
        _tasks.Add(taskName, task);
    }
    public void LoadTasks(Dictionary<string, Action> tasks)
    {
        foreach (var task in tasks)
        {
            AddTask(task.Key, task.Value);
        }
    }
    public void ClearTasks()
    {
        _tasks.Clear();
    }
    public void DeleteTask(string taskName)
    {
        if (_tasks.ContainsKey(taskName))
        {
            _tasks.Remove(taskName);
        }
    }
    public SuperDuperMenu(Dictionary<string, Action> tasks)
    {
        LoadTasks(tasks);
    }
    public SuperDuperMenu(){}
    public void Run()
    {
        this.AddTask("Exit", () => Environment.Exit(0));
        int selectedTaskIndex = 0;

        while (true)
        {
            Console.Title = "Main menu";
            Console.Clear();
            Console.CursorVisible = false;

            for (int i = 0; i < _tasks.Count; i++)
            {
                Console.ResetColor();
                if (i == selectedTaskIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("-> " + _tasks.ElementAt(i).Key);
                }
                else
                {
                    Console.WriteLine("   " + _tasks.ElementAt(i).Key);
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedTaskIndex = (selectedTaskIndex - 1 + _tasks.Count) % _tasks.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selectedTaskIndex = (selectedTaskIndex + 1) % _tasks.Count;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Console.CursorVisible = true;
                    _tasks.ElementAt(selectedTaskIndex).Value();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    break;
            }
        }
    }
}
