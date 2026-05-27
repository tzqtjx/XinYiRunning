using Microsoft.Maui.Storage;

namespace RecordTableApp.Services;

public class AppState
{
    private readonly ServerService _server;

    public AppState(ServerService server)
    {
        _server = server;
        LoadLocalPersons();
    }

    public List<string> SelectedPersons { get; set; } = new();
    public int TotalLaps { get; set; } = 0;
    public string Mode { get; set; } = "";
    public bool IsRunning { get; set; } = false;
    public bool IsPaused { get; set; } = false;
    public DateTime StartTime { get; set; }
    public TimeSpan ElapsedTime { get; set; }
    public Dictionary<string, int> PersonLaps { get; set; } = new();
    public Dictionary<string, TimeSpan> PersonTimes { get; set; } = new();

    public List<string> AllPersons { get; set; } = new();
    public List<TestRecord> Records { get; set; } = new();

    public List<string> AllPersonsTc { get; set; } = new();
    public List<TestRecord> RecordsTc { get; set; } = new();

    public List<LeaderboardEntry> Leaderboard { get; set; } = new();

    public bool ServerOnline { get; set; } = false;

    public async Task CheckServerAsync()
    {
        ServerOnline = await _server.PingAsync();
        if (ServerOnline)
        {
            Records = await _server.GetMingzhiRecordsAsync();
            RecordsTc = await _server.GetTiancaiRecordsAsync();
            Leaderboard = await _server.GetLeaderboardAsync();
        }
    }

    public async Task RefreshMingzhiAsync()
    {
        Records = await _server.GetMingzhiRecordsAsync();
    }

    public async Task RefreshTiancaiAsync()
    {
        RecordsTc = await _server.GetTiancaiRecordsAsync();
    }

    public async Task RefreshLeaderboardAsync()
    {
        Leaderboard = await _server.GetLeaderboardAsync();
    }

    public async Task SaveCurrentTest()
    {
        var completed = SelectedPersons
            .Where(p => PersonLaps.GetValueOrDefault(p) >= TotalLaps)
            .Select(p => new PersonResult
            {
                Name = p,
                Laps = PersonLaps.GetValueOrDefault(p),
                Time = PersonTimes.GetValueOrDefault(p)
            })
            .ToList();

        if (completed.Count == 0) return;

        var record = new TestRecord
        {
            Date = DateTime.Now,
            ModeLabel = Mode == "3km" ? "三公里" : $"{TotalLaps}圈",
            TotalLaps = TotalLaps,
            Results = completed
        };
        Records.Insert(0, record);
        await _server.SaveMingzhiRecordsAsync(Records);
    }

    public async Task SaveCurrentTestTc()
    {
        var completed = SelectedPersons
            .Where(p => PersonLaps.GetValueOrDefault(p) >= TotalLaps)
            .Select(p => new PersonResult
            {
                Name = p,
                Laps = PersonLaps.GetValueOrDefault(p),
                Time = PersonTimes.GetValueOrDefault(p)
            })
            .ToList();

        if (completed.Count == 0) return;

        var record = new TestRecord
        {
            Date = DateTime.Now,
            ModeLabel = Mode == "3km" ? "三公里" : $"{TotalLaps}圈",
            TotalLaps = TotalLaps,
            Results = completed
        };
        RecordsTc.Insert(0, record);
        await _server.SaveTiancaiRecordsAsync(RecordsTc);
    }

    public async Task SaveLeaderboardAsync()
    {
        await _server.SaveLeaderboardAsync(Leaderboard);
    }

    public void Reset()
    {
        SelectedPersons.Clear();
        TotalLaps = 0;
        Mode = "";
        IsRunning = false;
        IsPaused = false;
        ElapsedTime = TimeSpan.Zero;
        PersonLaps.Clear();
        PersonTimes.Clear();
    }

    public void SaveLocalPersons()
    {
        Preferences.Set("all_persons", string.Join(",", AllPersons));
        Preferences.Set("all_persons_tc", string.Join(",", AllPersonsTc));
    }

    private void LoadLocalPersons()
    {
        var persons = Preferences.Get("all_persons", "");
        if (!string.IsNullOrEmpty(persons))
            AllPersons = persons.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

        var personsTc = Preferences.Get("all_persons_tc", "");
        if (!string.IsNullOrEmpty(personsTc))
            AllPersonsTc = personsTc.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
