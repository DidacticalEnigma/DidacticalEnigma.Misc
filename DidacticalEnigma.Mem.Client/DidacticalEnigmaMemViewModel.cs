using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DidacticalEnigma.Mem.Client.MemApi;
using IdentityModel.Client;
using Utility.Utils;

namespace DidacticalEnigma.Mem.Client;

public class DidacticalEnigmaMemViewModel : INotifyPropertyChanged, IDisposable
{
    private HttpClient? httpClient;
    private readonly string clientId;
    private readonly List<string> additionalScopes;
    private string? refreshToken = null;
    private bool isLoggedIn;
    private string? verificationUri;
    private string? errorMessage;
    private string? userCode;
    private string? uri;
    private readonly RelayCommand initialize;
    private bool isLoggingIn;
    private readonly RelayCommand reset;
    private readonly RelayCommand logIn;
    private readonly RelayCommand logOut;

    public DidacticalEnigmaMemViewModel(string? clientId = null, IEnumerable<string>? additionalScopes = null)
    {
        this.clientId = clientId ?? "DidacticalEnigma";
        this.additionalScopes = (additionalScopes ?? Enumerable.Empty<string>()).ToList();
        this.initialize = new RelayCommand(() =>
        {
            var apiClient = new DidacticalEnigmaMem(new Uri(uri ?? throw new InvalidOperationException()));
            this.httpClient = apiClient.HttpClient;
            Client = apiClient;
        }, () => uri != null && !IsLoggedIn && !IsLoggingIn);
        this.reset = new RelayCommand(
            ResetAction,
            () => !IsLoggedIn && !IsLoggingIn);
        this.logIn = new RelayCommand(LogInMethod, () => !IsLoggedIn && !IsLoggingIn);
        this.logOut = new RelayCommand(LogOutMethod, () => IsLoggedIn);
        ClientAccessor = () => this.Client;
    }

    private async void LogOutMethod()
    {
        await this.TryLogout();
    }

    private async void LogInMethod()
    {
        await this.TryLogin();
    }

    private async void ResetAction()
    {
        await this.TryLogout();
        this.Dispose();
        ErrorMessage = null;
    }

    public string? Uri
    {
        get => uri;
        set
        {
            if (uri != value)
                return;
            uri = value;
            OnPropertyChanged();
        }
    }

    public ICommand Initialize => initialize;
    
    public ICommand Reset => initialize;
    
    public ICommand LogIn => logIn;
    
    public ICommand LogOut => logOut;

    public IDidacticalEnigmaMem? Client { get; private set; }

    public Func<IDidacticalEnigmaMem?> ClientAccessor { get; }

    public bool IsLoggingIn
    {
        get => isLoggingIn;
        private set
        {
            if (isLoggingIn != value)
                return;
            isLoggingIn = value;
            OnPropertyChanged();
        }
    }

    public string? UserCode
    {
        get => userCode;
        private set
        {
            if (userCode != value)
                return;
            userCode = value;
            OnPropertyChanged();
        }
    }

    public string? VerificationUri
    {
        get => verificationUri;
        private set
        {
            if (verificationUri != value)
                return;
            verificationUri = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoggedIn
    {
        get => isLoggedIn;
        private set
        {
            if (isLoggedIn != value)
                return;
            isLoggedIn = value;
            OnPropertyChanged();
        }
    }

    public string? ErrorMessage
    {
        get => errorMessage;
        private set
        {
            if (errorMessage != value)
                return;
            errorMessage = value;
            OnPropertyChanged();
        }
    }

    public async Task<bool> TryLogin(CancellationToken cancellationToken = default)
    {
        var discovery = await this.httpClient.GetDiscoveryDocumentAsync(uri.ToString(), cancellationToken: cancellationToken);
        if (discovery.IsError)
        {
            ErrorMessage = discovery.Error;
            return false;
        }

        var deviceResponse = await this.httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
        { 
            Address = discovery.DeviceAuthorizationEndpoint,
            Scope = string.Join(" ", additionalScopes.Concat(new string[]
            {
                "openid",
                "offline_access",
                "profile",
                "email"
            })),
            ClientId = clientId
        }, cancellationToken: cancellationToken);
        if (deviceResponse.IsError)
        {
            ErrorMessage = deviceResponse.Error;
            return false;
        }

        IsLoggingIn = true;
        VerificationUri = deviceResponse.VerificationUriComplete;
        UserCode = deviceResponse.UserCode;

        do
        {
            var tokenResponse = await httpClient.RequestDeviceTokenAsync(new DeviceTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = clientId,
                DeviceCode = deviceResponse.DeviceCode
            }, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            if (tokenResponse is { IsError: true, Error: "authorization_pending" })
            {
                await Task.Delay(Math.Clamp(deviceResponse.Interval, 1, 60) * 1000, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
            else if (tokenResponse.IsError)
            {
                ErrorMessage = tokenResponse.Error;
                return false;
            }
            else
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                refreshToken = tokenResponse.RefreshToken;
                break;
            }
        }
        while (true);
        
        IsLoggedIn = true;
        UserCode = null;
        VerificationUri = null;
        ErrorMessage = null;
        IsLoggingIn = false;
        return true;
    }

    public async Task<bool> TryLogout()
    {
        if (isLoggedIn && httpClient != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            refreshToken = null;
            IsLoggedIn = false;
        }

        return true;
    }

    public void Dispose()
    {
        Client?.Dispose();
        Client = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        this.initialize.OnExecuteChanged();
        this.reset.OnExecuteChanged();
        this.logIn.OnExecuteChanged();
        this.logOut.OnExecuteChanged();
    }
}