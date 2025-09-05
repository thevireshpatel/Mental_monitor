using MentalMonitor.Data;
using MentalMonitor.Models;

namespace MentalMonitor.Services;
public class EmotionService
{
    private readonly PythonBridgeService _py;
    public EmotionService(PythonBridgeService py) => _py = py;

    public async Task<List<EmotionScore>> AnalyzeEntryAsync(Entry entry)
    {
        var preds = await _py.PredictEmotionAsync(entry.Text);
        return preds.Select(kv => new EmotionScore
        {
            EntryId = entry.Id,
            EmotionLabel = kv.Key,
            Score = kv.Value
        }).ToList();
    }
}