using PersonalPortal.Core.Models;
using System.Collections.Concurrent;

namespace PersonalPortal.Web.Services;

public class AuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private UserInfo? _currentUser;
    private string? _token;
    private readonly object _lock = new object();

    public event Action? OnAuthStateChanged;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public UserInfo? CurrentUser 
    { 
        get 
        { 
            lock (_lock) 
            { 
                return _currentUser; 
            } 
        } 
    }
    
    public bool IsAuthenticated 
    { 
        get 
        { 
            lock (_lock) 
            { 
                return _currentUser != null; 
            } 
        } 
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("ApiClient");
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            
            if (!response.IsSuccessStatusCode)
            {
                return new LoginResponse 
                { 
                    Success = false, 
                    Message = $"API returned error: {response.StatusCode}" 
                };
            }
            
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse?.Success == true && loginResponse.User != null)
            {
                lock (_lock)
                {
                    _currentUser = loginResponse.User;
                    _token = loginResponse.Token;
                }

                OnAuthStateChanged?.Invoke();
            }

            return loginResponse ?? new LoginResponse { Success = false, Message = "Invalid response from server" };
        }
        catch (HttpRequestException ex)
        {
            return new LoginResponse 
            { 
                Success = false, 
                Message = $"Cannot connect to API. Please ensure the API is running. Error: {ex.Message}" 
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse 
            { 
                Success = false, 
                Message = $"Login error: {ex.Message}" 
            };
        }
    }

    public void Logout()
    {
        lock (_lock)
        {
            _currentUser = null;
            _token = null;
        }
        OnAuthStateChanged?.Invoke();
    }
}
