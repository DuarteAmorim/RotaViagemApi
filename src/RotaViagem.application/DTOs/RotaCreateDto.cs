namespace RotaViagem.application.DTOs
{
    public class RotaCreateDto
    {
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Preco { get; set; }
    }
}
