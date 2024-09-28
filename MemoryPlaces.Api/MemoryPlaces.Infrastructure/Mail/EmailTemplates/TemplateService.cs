using MemoryPlaces.Application.Interfaces;

public class TemplateService : ITemplateService
{
    public async Task<string> LoadConfirmAccountTemplateAsync(string id)
    {
        var templatePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "../MemoryPlaces.Infrastructure/Mail/EmailTemplates",
            "ConfirmAccountTemplate.html"
        );
        var template = await File.ReadAllTextAsync(templatePath);

        var renderedTemplate = template.Replace("@activationLink", id);

        return renderedTemplate;
    }
}
