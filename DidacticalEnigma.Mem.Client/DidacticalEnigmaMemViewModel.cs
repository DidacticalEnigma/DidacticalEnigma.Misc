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
using Optional;
using Optional.Unsafe;
using Utility.Utils;

namespace DidacticalEnigma.Mem.Client;

public class DidacticalEnigmaMemViewModel : INotifyPropertyChanged, IDisposable
{
    private HttpClient? httpClient;
    private readonly string clientId;
    private readonly List<string> additionalScopes;
    private string? refreshToken = null;
    private bool isSet;
    private bool isLoggedIn;
    private bool isLoggingIn;
    private string? verificationUri;
    private string? errorMessage;
    private string? userCode;
    private string? uri;
    private readonly RelayCommand initialize;
    private readonly RelayCommand reset;
    private readonly RelayCommand logIn;
    private readonly RelayCommand logOut;

    public DidacticalEnigmaMemViewModel(string? clientId = null, IEnumerable<string>? additionalScopes = null)
    {
        this.clientId = clientId ?? "DidacticalEnigma";
        this.additionalScopes = (additionalScopes ?? Enumerable.Empty<string>()).ToList();
        this.initialize = new RelayCommand(InitializeMethod, () => !string.IsNullOrWhiteSpace(uri) && !IsSet && !IsLoggedIn && !IsLoggingIn);
        this.reset = new RelayCommand(ResetMethod, () => IsSet && !IsLoggedIn && !IsLoggingIn);
        this.logIn = new RelayCommand(LogInMethod, () => IsSet && !IsLoggedIn && !IsLoggingIn);
        this.logOut = new RelayCommand(LogOutMethod, () => IsSet && IsLoggedIn);
        ClientAccessor = () => this.Client;
    }

    private void InitializeMethod()
    {
        var apiClient = new DidacticalEnigmaMem(new Uri(uri ?? throw new InvalidOperationException()));
        this.httpClient = apiClient.HttpClient;
        Client = apiClient;
        IsSet = true;
    }

    private async void LogOutMethod()
    {
        await this.TryLogout();
    }

    private async void LogInMethod()
    {
        await this.TryLogin();
    }

    private async void ResetMethod()
    {
        await this.TryLogout();
        this.Dispose();
        ErrorMessage = null;
        IsSet = false;
    }

    public string? Uri
    {
        get => uri;
        set
        {
            if (uri == value)
                return;
            uri = value;
            OnPropertyChanged();
        }
    }

    public ICommand Initialize => initialize;

    public ICommand Reset => reset;

    public ICommand LogIn => logIn;

    public ICommand LogOut => logOut;

    public IDidacticalEnigmaMem? Client { get; private set; }

    public Func<IDidacticalEnigmaMem?> ClientAccessor { get; }

    public bool IsNotSet => !IsSet;

    public bool IsSet
    {
        get => isSet;
        private set
        {
            if (isSet == value)
                return;
            isSet = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotSet));
        }
    }

    public bool IsLoggingIn
    {
        get => isLoggingIn;
        private set
        {
            if (isLoggingIn == value)
                return;
            isLoggingIn = value;
            OnPropertyChanged();
        }
    }

    public string? Prompt =>
        UserCode != null ? $"The code is {UserCode}. Follow the link to continue authorization process:" : null;

    public string? UserCode
    {
        get => userCode;
        private set
        {
            if (userCode == value)
                return;
            userCode = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Prompt));
        }
    }

    public string? VerificationUri
    {
        get => verificationUri;
        private set
        {
            if (verificationUri == value)
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
            if (isLoggedIn == value)
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
            if (errorMessage == value)
                return;
            errorMessage = value;
            OnPropertyChanged();
        }
    }

    public async Task<bool> TryLogin(CancellationToken cancellationToken = default)
    {
        var resultOpt = await httpClient.StartDeviceCodeLoginProcess(
            new Uri(uri),
            clientId,
            additionalScopes,
            cancellationToken);

        if (resultOpt.HasValue)
        {
            var result = resultOpt.ValueOrFailure();
            IsLoggingIn = true;
            VerificationUri = result.VerificationUriComplete.ToString();
            UserCode = result.UserCode;

            var processResultOpt = await result.ProcessResultTask;

            return processResultOpt.Match(
                processResult =>
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", processResult.AccessToken);
                    UserCode = null;
                    VerificationUri = null;
                    ErrorMessage = null;
                    IsLoggingIn = false;
                    IsLoggedIn = true;
                    refreshToken = processResult.RefreshToken;
                    return true;
                },
                err =>
                {
                    ErrorMessage = err;
                    UserCode = null;
                    VerificationUri = null;
                    IsLoggingIn = false;
                    return false;
                });
        }
        else
        {
            resultOpt.MatchNone(err =>
            {
                ErrorMessage = err;
            });
            return false;
        }
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