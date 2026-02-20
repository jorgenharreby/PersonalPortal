using PersonalPortal.Core.Models;

namespace PersonalPortal.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // TextNotes
    public async Task<List<TextNote>> GetAllTextNotesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TextNote>>("api/textnotes") ?? new();
    }

    public async Task<TextNote?> GetTextNoteByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<TextNote>($"api/textnotes/{id}");
    }

    public async Task<List<TextNote>> GetLatestTextNotesAsync(int count)
    {
        return await _httpClient.GetFromJsonAsync<List<TextNote>>($"api/textnotes/latest/{count}") ?? new();
    }

    public async Task<List<TextNote>> SearchTextNotesAsync(string searchTerm)
    {
        return await _httpClient.GetFromJsonAsync<List<TextNote>>($"api/textnotes/search/{searchTerm}") ?? new();
    }

    public async Task<Guid?> CreateTextNoteAsync(TextNote note)
    {
        var response = await _httpClient.PostAsJsonAsync("api/textnotes", note);
        return await response.Content.ReadFromJsonAsync<Guid?>();
    }

    public async Task<bool> UpdateTextNoteAsync(Guid id, TextNote note)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/textnotes/{id}", note);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTextNoteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/textnotes/{id}");
        return response.IsSuccessStatusCode;
    }

    // Checklists
    public async Task<List<Checklist>> GetAllChecklistsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Checklist>>("api/checklists") ?? new();
    }

    public async Task<Checklist?> GetChecklistByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Checklist>($"api/checklists/{id}");
    }

    public async Task<List<Checklist>> GetLatestChecklistsAsync(int count)
    {
        return await _httpClient.GetFromJsonAsync<List<Checklist>>($"api/checklists/latest/{count}") ?? new();
    }

    public async Task<List<Checklist>> GetChecklistsByTypeAsync(string type)
    {
        return await _httpClient.GetFromJsonAsync<List<Checklist>>($"api/checklists/type/{type}") ?? new();
    }

    public async Task<List<Checklist>> SearchChecklistsAsync(string searchTerm)
    {
        return await _httpClient.GetFromJsonAsync<List<Checklist>>($"api/checklists/search/{searchTerm}") ?? new();
    }

    public async Task<Guid?> CreateChecklistAsync(Checklist checklist)
    {
        var response = await _httpClient.PostAsJsonAsync("api/checklists", checklist);
        return await response.Content.ReadFromJsonAsync<Guid?>();
    }

    public async Task<bool> UpdateChecklistAsync(Guid id, Checklist checklist)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/checklists/{id}", checklist);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteChecklistAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/checklists/{id}");
        return response.IsSuccessStatusCode;
    }

    // Recipes
    public async Task<List<Recipe>> GetAllRecipesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Recipe>>("api/recipes") ?? new();
    }

    public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Recipe>($"api/recipes/{id}");
    }

    public async Task<List<Recipe>> GetLatestRecipesAsync(int count)
    {
        return await _httpClient.GetFromJsonAsync<List<Recipe>>($"api/recipes/latest/{count}") ?? new();
    }

    public async Task<List<Recipe>> GetRecipesByTypeAsync(string type)
    {
        return await _httpClient.GetFromJsonAsync<List<Recipe>>($"api/recipes/type/{type}") ?? new();
    }

    public async Task<List<Recipe>> SearchRecipesAsync(string searchTerm)
    {
        return await _httpClient.GetFromJsonAsync<List<Recipe>>($"api/recipes/search/{searchTerm}") ?? new();
    }

    public async Task<Guid?> CreateRecipeAsync(Recipe recipe)
    {
        var response = await _httpClient.PostAsJsonAsync("api/recipes", recipe);
        return await response.Content.ReadFromJsonAsync<Guid?>();
    }

    public async Task<bool> UpdateRecipeAsync(Guid id, Recipe recipe)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/recipes/{id}", recipe);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteRecipeAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/recipes/{id}");
        return response.IsSuccessStatusCode;
    }

    // Pictures
    public async Task<List<Picture>> GetAllPicturesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Picture>>("api/pictures") ?? new();
    }

    public async Task<Picture?> GetPictureByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Picture>($"api/pictures/{id}");
    }

    public async Task<List<Picture>> GetLatestPicturesAsync(int count)
    {
        return await _httpClient.GetFromJsonAsync<List<Picture>>($"api/pictures/latest/{count}") ?? new();
    }

    public async Task<List<Picture>> GetPicturesByRecipeIdAsync(Guid recipeId)
    {
        return await _httpClient.GetFromJsonAsync<List<Picture>>($"api/pictures/recipe/{recipeId}") ?? new();
    }

    public async Task<Guid?> CreatePictureAsync(Picture picture)
    {
        var response = await _httpClient.PostAsJsonAsync("api/pictures", picture);
        return await response.Content.ReadFromJsonAsync<Guid?>();
    }

    public async Task<bool> UpdatePictureAsync(Guid id, Picture picture)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/pictures/{id}", picture);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePictureAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/pictures/{id}");
        return response.IsSuccessStatusCode;
    }
}
