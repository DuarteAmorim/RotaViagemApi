using RotaViagem.application.Interfaces;
using RotaViagem.application.Utils;
using RotaViagem.domain.Entities;

namespace RotaViagem.application.Services
{
    /// <summary>
    /// Serviço para manipulação de rotas de viagem, incluíndo persistência e cálculo de rotas mais baratas.
    /// </summary>
    public class RotaService : IRotaService
    {
        private readonly IDataService<Rota> _iDataService;
        private List<Rota> _RotasData = new();

        public RotaService(IDataService<Rota> dataService)
        {
            _iDataService = dataService;
        }

        private async Task LoadDataAsync()
        {
            if (_RotasData.Count == 0)
            {
                _RotasData = await _iDataService.LoadAsync();
            }
        }
        private async Task SaveDataAsync()
        {
            await _iDataService.SaveAsync(_RotasData);
        }
        public async Task<IEnumerable<Rota>> GetAllAsync()
        {
            await LoadDataAsync();
            return _RotasData;
        }

        public async Task<Rota?> GetByIdAsync(string id)
        {
            await LoadDataAsync();
            if (!Guid.TryParse(id, out var guid))
                return null;
            return _RotasData.FirstOrDefault(r => r.Id == guid);
        }

        public async Task<IEnumerable<Rota>> GetByOrigemDestinoAsync(string origem, string destino)
        {
            await LoadDataAsync();
            origem = origem.ToUpperInvariant();
            destino = destino.ToUpperInvariant();

            return _RotasData.Where(r => r.Origem == origem && r.Destino == destino);

            //return _RotasData
            //    .Where(r => r.Origem.Equals(origem, StringComparison.OrdinalIgnoreCase) &&
            //                r.Destino.Equals(destino, StringComparison.OrdinalIgnoreCase))
            //    .ToList();
        }

        public async Task<(IEnumerable<string> rota, double custo)> FindRotaMaisBarata(string origem, string destino)
        {
            await LoadDataAsync();

            var grafo = new Dictionary<string, List<(string destino, double custo)>>();

            foreach (var rota in _RotasData)
            {
                if (!grafo.ContainsKey(rota.Origem))
                    grafo[rota.Origem] = new List<(string, double)>();

                grafo[rota.Origem].Add((rota.Destino, rota.Preco));
            }

            var dijkstra = new DijkstraService(grafo);
            return dijkstra.FindRotaMaisBarata(origem.ToUpperInvariant(), destino.ToUpperInvariant());
        }

        public async Task<Rota?> CreateAsync(Rota rota)
        {
            await LoadDataAsync();

            _RotasData.Add(rota);

            await SaveDataAsync();
            return rota;
        }
        public async Task<bool> UpdateAsync(string id, Rota rota)
        {

            await LoadDataAsync();

            if (!Guid.TryParse(id, out var parsedGuid)) return false;

            var rotaExistente = _RotasData.FirstOrDefault(r => r.Id == parsedGuid);

            if (rotaExistente == null) return false;

            rotaExistente.Atualizar(rota.Origem, rota.Destino, rota.Preco);

            await SaveDataAsync();

            return true;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            await LoadDataAsync();
            if (!Guid.TryParse(id, out var guid))
                return false;

            var existente = _RotasData.FirstOrDefault(r => r.Id == guid);
            if (existente == null)
                return false;

            _RotasData.Remove(existente);
            await SaveDataAsync();
            return true;
        }

    }
}
