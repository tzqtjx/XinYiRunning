using System.Net.Http.Json;
using RecordTableApp.Services;

namespace RecordTableApp.Services;

public class ServerService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "http://192.168.110.210:7777";

    public ServerService()
    {
        _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };
    }

    public async Task<bool> PingAsync()
    {
        try
        {
            var resp = await _http.GetAsync($"{BaseUrl}/api/ping");
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<TestRecord>> GetMingzhiRecordsAsync()
    {
        try
        {
            var data = await _http.GetFromJsonAsync<List<ServerTestRecord>>($"{BaseUrl}/api/mingzhi");
            return data?.Select(ToClientRecord).ToList() ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> SaveMingzhiRecordsAsync(List<TestRecord> records)
    {
        try
        {
            var data = records.Select(ToServerRecord).ToList();
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/mingzhi", data);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<TestRecord>> GetTiancaiRecordsAsync()
    {
        try
        {
            var data = await _http.GetFromJsonAsync<List<ServerTestRecord>>($"{BaseUrl}/api/tiancai");
            return data?.Select(ToClientRecord).ToList() ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> SaveTiancaiRecordsAsync(List<TestRecord> records)
    {
        try
        {
            var data = records.Select(ToServerRecord).ToList();
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/tiancai", data);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboardAsync()
    {
        try
        {
            var data = await _http.GetFromJsonAsync<List<ServerLeaderboardEntry>>($"{BaseUrl}/api/leaderboard");
            return data?.Select(ToClientEntry).ToList() ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> SaveLeaderboardAsync(List<LeaderboardEntry> entries)
    {
        try
        {
            var data = entries.Select(ToServerEntry).ToList();
            var resp = await _http.PostAsJsonAsync($"{BaseUrl}/api/leaderboard", data);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static TestRecord ToClientRecord(ServerTestRecord r) => new()
    {
        Date = DateTime.TryParse(r.Date, out var d) ? d : DateTime.Now,
        ModeLabel = r.ModeLabel,
        TotalLaps = r.TotalLaps,
        Results = r.Results.Select(p => new PersonResult
        {
            Name = p.Name,
            Laps = p.Laps,
            Time = TimeSpan.FromSeconds(p.TimeSeconds)
        }).ToList()
    };

    private static ServerTestRecord ToServerRecord(TestRecord r) => new()
    {
        Date = r.Date.ToString("O"),
        ModeLabel = r.ModeLabel,
        TotalLaps = r.TotalLaps,
        Results = r.Results.Select(p => new ServerPersonResult
        {
            Name = p.Name,
            Laps = p.Laps,
            TimeSeconds = p.Time.TotalSeconds
        }).ToList()
    };

    private static LeaderboardEntry ToClientEntry(ServerLeaderboardEntry e) => new()
    {
        Name = e.Name,
        ApproxDate = e.ApproxDate,
        Time = TimeSpan.FromSeconds(e.TimeSeconds)
    };

    private static ServerLeaderboardEntry ToServerEntry(LeaderboardEntry e) => new()
    {
        Name = e.Name,
        ApproxDate = e.ApproxDate,
        TimeSeconds = e.Time.TotalSeconds
    };
}
