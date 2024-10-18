using Newtonsoft.Json;

namespace Ecoeden.Inventory.Application.Helpers;
public static class FileReaderHelper<T>  where T : class
{
    public static List<T> ReadFile(string fileName, string path)
    {
        var data = File.ReadAllText($"{path}/{fileName}.json");
        var jsonData = JsonConvert.DeserializeObject<List<T>>(data);

        return jsonData;
    }
}
