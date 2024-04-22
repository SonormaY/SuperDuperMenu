using System.Text.Json;

namespace SuperDuperMenuLib;

public class SuperDuperMenu
{
    public string Title { get; set; } = "Main menu";
    public string ExitMessage { get; set; } = "Press any key to exit...";
    public ConsoleColor ExitMessageColor { get; set; } = ConsoleColor.DarkCyan;
    public ConsoleColor SelectionColor { get; set; } = ConsoleColor.Cyan;
    public bool CursorVisible { get; set; } = false;
    private readonly Dictionary<string, Action> _entries = new();
    public void AddEntry(string entryName, Action task, bool overwrite = false)
    {
        if (_entries.ContainsKey(entryName) && !overwrite)
        {
            throw new ArgumentException("An entry with the same name already exists.");
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
    public void SaveConfig(string path = "SDMConfig.json")
    {
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
    public void LoadConfig(string path = "SDMConfig.json")
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SuperDuperMenu config = JsonSerializer.Deserialize<SuperDuperMenu>(json);
            this.Title = config.Title;
            this.ExitMessage = config.ExitMessage;
            this.ExitMessageColor = config.ExitMessageColor;
            this.SelectionColor = config.SelectionColor;
            this.CursorVisible = config.CursorVisible;
        }
    }
    public SuperDuperMenu(Dictionary<string, Action> entries)
    {
        LoadEntries(entries);
    }
    public SuperDuperMenu(){}
    public void Run()
    {
        bool exit = false;
        this.AddEntry("Exit", () => exit = true, true);
        int selectedTaskIndex = 0;

        while (!exit)
        {
            Console.Title = Title;
            Console.Clear();
            Console.CursorVisible = CursorVisible;

            for (int i = 0; i < _entries.Count; i++)
            {
                Console.ResetColor();
                if (i == selectedTaskIndex)
                {
                    Console.ForegroundColor = SelectionColor;
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
                    if (exit) break;
                    Console.ForegroundColor = ExitMessageColor;
                    Console.WriteLine(ExitMessage);
                    Console.CursorVisible = false;
                    Console.ReadKey();
                    break;
            }
        }
    }
}
