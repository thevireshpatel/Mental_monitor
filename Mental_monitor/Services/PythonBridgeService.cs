using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace MentalMonitor.Services;
public class PythonBridgeService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _pyExe;

    public PythonBridgeService(IWebHostEnvironment env, IConfiguration cfg)
    {
        _env = env;
        _pyExe = cfg["PythonPath"] ?? "python";   // fallback to PATH
    }

    public async Task<Dictionary<string, double>> PredictEmotionAsync(string text)
    {
        var root = Path.Combine(_env.ContentRootPath, "PythonBridge");
        var script = Path.Combine(root, "predict_emotion.py");

        var psi = new ProcessStartInfo
        {
            FileName = _pyExe,
            Arguments = $"\"{script}\"",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi) ?? throw new InvalidOperationException("Cannot start Python");
        await proc.StandardInput.WriteLineAsync(text);
        proc.StandardInput.Close();

        var json = await proc.StandardOutput.ReadToEndAsync();
        await proc.WaitForExitAsync();

        if (proc.ExitCode != 0)
        {
            var err = await proc.StandardError.ReadToEndAsync();
            throw new Exception($"Python error: {err}");
        }

        return JsonSerializer.Deserialize<Dictionary<string, double>>(json)
               ?? new Dictionary<string, double>();
    }
}