//Helper class for generating unique ids and delaying execution

public static class Utils
{
    public static string GenerateId()
    {
        return Guid.NewGuid().ToString();
    }

    public static async Task Delay(int ms)
    {
        await Task.Delay(ms);
    }
}