using System.Text.Json.Serialization;

namespace RotaViagem.domain.Entities
{
    public class Rota
    {
        /// <summary>
        /// Identificador único da rota.
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// Sigla do aeroporto de origem (3 caracteres maiúscula).
        /// </summary>
        public string Origem { get; private set; }

        /// <summary>
        /// Sigal do aeroporto de destino (3 caracteres maiúscula).
        /// </summary>
        public string Destino { get; private set; }

        /// <summary>
        /// Preço do vôo.
        /// </summary>
        public double Preco { get; private set; }

        /// <summary>
        /// Criar nova rota com geração automática de Id.
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <param name="preco"></param>
        public Rota(string origem, string destino, double preco)
        {
            Id = Guid.NewGuid();
            Origem = origem.ToUpperInvariant();
            Destino = destino.ToUpperInvariant();
            Preco = preco;
        }

        /// <summary>
        /// Construtor usado para desserialização de JSON.
        /// </summary>
        [JsonConstructor]
        public Rota(Guid id, string origem, string destino, double preco)
        {
            Id = id;
            Origem = origem.ToUpperInvariant();
            Destino = destino.ToUpperInvariant();
            Preco = preco;
        }

        /// <summary>
        /// Construtor usado para atualizar a rota.
        /// </summary>
        public void Atualizar(string origem, string destino, double preco)
        {
            Origem = origem.ToUpperInvariant();
            Destino = destino.ToUpperInvariant();
            Preco = preco;
        }
    }
}
