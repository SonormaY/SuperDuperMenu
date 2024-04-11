namespace SuperDuperMenuLib;

public class SuperDuperMenu
{
    private readonly Dictionary<string, Action> _entries = new();
    public void AddEntry(string entryName, Action task)
    {
        if (_entries.ContainsKey(entryName))
        {
            return;
        }

        entryName = entryName.First().ToString().ToUpper() + entryName.Substring(1);
        _entries.Add(entryName, task);
    }
    public void LoadEntries(Dictionary<string, Action> entries)
    {
        foreach (var entry in entries)
        {
            AddEntry(entry.Key, entry.Value);
        }
    }
    public void ClearEntries()
    {
        _entries.Clear();
    }
    public void DeleteEntry(string entryName)
    {
        if (_entries.ContainsKey(entryName))
        {
            _entries.Remove(entryName);
        }
    }
    public SuperDuperMenu(Dictionary<string, Action> entries)
    {
        LoadEntries(entries);
    }
    public SuperDuperMenu(){}
    public void Run()
    {
        this.AddEntry("Exit", () => Environment.Exit(0));
        int selectedTaskIndex = 0;

        while (true)
        {
            Console.Title = "Main menu";
            Console.Clear();
            Console.CursorVisible = false;

            for (int i = 0; i < _entries.Count; i++)
            {
                Console.ResetColor();
                if (i == selectedTaskIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("-> " + _entries.ElementAt(i).Key);
                }
                else
                {
                    Console.WriteLine("   " + _entries.ElementAt(i).Key);
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedTaskIndex = (selectedTaskIndex - 1 + _entries.Count) % _entries.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selectedTaskIndex = (selectedTaskIndex + 1) % _entries.Count;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    Console.CursorVisible = true;
                    _entries.ElementAt(selectedTaskIndex).Value();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    break;
            }
        }
    }
}
