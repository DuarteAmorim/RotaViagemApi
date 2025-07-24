using RotaViagem.application.Interfaces;

namespace RotaViagem.application.Utils
{
    /// <summary>
    /// Serviço de dados que persiste em arquivo JSON.
    /// </summary>
    /// <typeparam name="T">Entidade a ser persistida e consultada</typeparam>
    public class JsonDataService<T> : IDataService<T>
    {

        private readonly string _filePath;

        public JsonDataService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<T>> LoadAsync()
        {
            if (!System.IO.File.Exists(_filePath))
            {
                return new List<T>();
            }
            var jsonData = File.ReadAllText(_filePath);
            var data = System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            return data;
        }

        public async Task SaveAsync(List<T> data)
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true // Formata o JSON para facilitar a leitura
            });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
