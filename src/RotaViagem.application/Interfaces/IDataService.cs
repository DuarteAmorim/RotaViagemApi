namespace RotaViagem.application.Interfaces
{
    /// <summary>
    /// Interface genérica para serviços de persistência de dados.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade a ser persistida</typeparam>
    public interface IDataService<T>
    {
        /// <summary>
        /// Carrega a lista de entidades.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> LoadAsync();
        /// <summary>
        /// Salvar a lista de entidades.
        /// </summary>
        Task SaveAsync(List<T> data);
    }
}
