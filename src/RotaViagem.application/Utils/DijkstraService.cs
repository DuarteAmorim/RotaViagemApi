using RotaViagem.application.Interfaces;

namespace RotaViagem.application.Utils
{
    public class DijkstraService : IDijkstraService
    

    {
        private readonly Dictionary<string, List<(string destino, double custo)>> _grafo;

        /// <summary>
        /// Inicializa o serviço com o grafo de rotas.
        /// </summary>
        /// <param name="grafo">Dicionário representando o grafo (origem -> lista de destinos e custos).</param>
        public DijkstraService(Dictionary<string, List<(string destino, double custo)>> grafo)
        {
            _grafo = grafo;
        }

        /// <summary>
        /// Encontra a rota mais barata entre origem e destino.
        /// </summary>
        /// <param name="origem">Sigla do aeroporto de origem.</param>
        /// <param name="destino">Sigla do aeroporto de destino.</param>
        /// <returns>Tupla com a lista de aeroportos da rota e o custo total.</returns>
        public (List<string> rota, double custo) FindRotaMaisBarata(string origem, string destino)
        {
            var vertices = new HashSet<string>();

            foreach (var v in _grafo.Keys)
            {
                vertices.Add(v);
                foreach (var (dest, _) in _grafo[v])
                {
                    vertices.Add(dest);
                }
            }

            var distancias = new Dictionary<string, double>();
            var anteriores = new Dictionary<string, string?>();
            var visitados = new HashSet<string>();
            var fila = new PriorityQueue<string, double>();

            foreach (var vertice in vertices)
            {
                distancias[vertice] = double.PositiveInfinity;
                anteriores[vertice] = null;
            }

            distancias[origem] = 0;
            fila.Enqueue(origem, 0);

            while (fila.Count > 0)
            {
                var atual = fila.Dequeue();

                if (atual == destino)
                    break;

                if (visitados.Contains(atual))
                    continue;

                visitados.Add(atual);

                if (!_grafo.ContainsKey(atual))
                    continue;

                foreach (var (vizinho, custo) in _grafo[atual])
                {
                    var novaDistancia = distancias[atual] + custo;
                    if (novaDistancia < distancias[vizinho])
                    {
                        distancias[vizinho] = novaDistancia;
                        anteriores[vizinho] = atual;
                        fila.Enqueue(vizinho, novaDistancia);
                    }
                }
            }

            if (!distancias.TryGetValue(destino, out var distanciaDestino))
            {
                return (new List<string>(), double.PositiveInfinity);
            }

            if (double.IsInfinity(distanciaDestino))
                return (new List<string>(), double.PositiveInfinity);

            var rota = new List<string>();
            var atualVertice = destino;
            while (atualVertice != null)
            {
                rota.Insert(0, atualVertice);
                atualVertice = anteriores[atualVertice];
            }

            return (rota, distanciaDestino);
        }
    }
}
