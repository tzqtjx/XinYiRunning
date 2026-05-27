namespace RecordTableApp.Services;

public class LeaderboardEntry
{
    public string Name { get; set; } = "";
    public string ApproxDate { get; set; } = "";
    public TimeSpan Time { get; set; }
}

public class PersonResult
{
    public string Name { get; set; } = "";
    public int Laps { get; set; }
    public TimeSpan Time { get; set; }
}

public class TestRecord
{
    public DateTime Date { get; set; }
    public string ModeLabel { get; set; } = "";
    public int TotalLaps { get; set; }
    public List<PersonResult> Results { get; set; } = new();
}

public class ServerLeaderboardEntry
{
    public string Name { get; set; } = "";
    public string ApproxDate { get; set; } = "";
    public double TimeSeconds { get; set; }
}

public class ServerPersonResult
{
    public string Name { get; set; } = "";
    public int Laps { get; set; }
    public double TimeSeconds { get; set; }
}

public class ServerTestRecord
{
    public string Date { get; set; } = "";
    public string ModeLabel { get; set; } = "";
    public int TotalLaps { get; set; }
    public List<ServerPersonResult> Results { get; set; } = new();
}
