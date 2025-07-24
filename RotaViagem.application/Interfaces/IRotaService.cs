using RotaViagem.domain.Entities;

namespace RotaViagem.application.Interfaces
{
    /// <summary>
    /// Interface para operações de CRUD e consultas de rotas de viagem.
    /// </summary>
    public interface IRotaService
    {
        /// <summary>
        /// Obtém todas as rotas cadastradas.
        /// </summary>
        Task<IEnumerable<Rota>> GetAllAsync();
        /// <summary>
        /// Obter rota pelo identificador.
        /// </summary>
        Task<Rota?> GetByIdAsync(string id);

        /// <summary>
        /// Obtém rotas filtrando por origem e destino.
        /// </summary>
        Task<IEnumerable<Rota>> GetByOrigemDestinoAsync(string origem, string destino);
        /// <summary>
        /// Cria uma nova rota de viagem.
        /// </summary>
        Task<Rota?> CreateAsync(Rota rota);
        /// <summary>
        /// Atualiza Rota existente.
        /// </summary>
        Task<bool> UpdateAsync(string id, Rota rota);
        /// <summary>
        /// Remove rota pelo identificador.
        /// </summary>
        Task<bool> DeleteAsync(string id);
        /// <summary>
        /// Encontra a rota mais barata entre dois trechos
        /// </summary>
        Task<(IEnumerable<string> rota, double custo)> FindRotaMaisBarata(string origem, string destino);
    }
}
