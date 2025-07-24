namespace RotaViagem.application.Interfaces
{
    public interface IDijkstraService
    {
        /// <summary>
        /// Encontra a rota mais barata entre dois pontos utilizando o algoritmo de Dijkstra.
        /// </summary>
        /// <param name="origem">Sigla do aeroporto de origem</param>
        /// <param name="destino">Sigla do aeroporto de destino</param>
        /// <returns>Tupla com a lista de aeroportos da rota e o curto total</returns>
        (List<string> rota, double custo) FindRotaMaisBarata(string origem, string destino);
    }
}
