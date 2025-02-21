namespace SalesAPI.Dtos;

public record CriarVendaDto(int ClienteId, int FilialId, List<VendaItemDto> Itens);
