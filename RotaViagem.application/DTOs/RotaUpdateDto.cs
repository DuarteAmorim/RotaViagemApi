namespace RotaViagem.application.DTOs
{
    public class RotaUpdateDto
    {
        public string Id { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public double Preco { get; set; }
    }
}
