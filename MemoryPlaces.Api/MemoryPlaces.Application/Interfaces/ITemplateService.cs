namespace MemoryPlaces.Application.Interfaces;

public interface ITemplateService
{
    Task<string> LoadConfirmAccountTemplateAsync(string id);
}
