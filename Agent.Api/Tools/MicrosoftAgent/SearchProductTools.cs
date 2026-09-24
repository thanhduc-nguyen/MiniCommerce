using System.ComponentModel;

namespace Agent.Api.Tools.MicrosoftAgent;

public static class SearchProductTool
{
    [Description("Get the weather for a given location.")]
    public static string GetWeather(string location)
    {
        return $"The weather in {location} is cloudy with a high of 15°C.";
    }

    [Description("Get the change of climate change of a given location.")]
    public static string GetClimateChange(string location)
    {
        return $"The climate change impact in {location} is significant with rising temperatures.";
    }
}