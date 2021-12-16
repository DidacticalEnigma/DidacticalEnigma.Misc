using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Optional;

namespace DidacticalEnigma.Mem.Client;

public static class DeviceCodeClient
{
    public static async Task<Option<DeviceCodeLoginProcess, string>> StartDeviceCodeLoginProcess(
        this HttpClient httpClient,
        Uri uri,
        string clientId,
        IEnumerable<string>? additionalScopes = null,
        CancellationToken cancellationToken = default)
    {
        var discovery = await httpClient.GetDiscoveryDocumentAsync(uri.ToString(), cancellationToken: cancellationToken);
        if (discovery.IsError)
        {
            return Option.None<DeviceCodeLoginProcess, string>(discovery.Error);
        }

        var deviceResponse = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
        {
            Address = discovery.DeviceAuthorizationEndpoint,
            Scope = string.Join(" ", (additionalScopes ?? Enumerable.Empty<string>()).Concat(new string[]
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
            return Option.None<DeviceCodeLoginProcess, string>(deviceResponse.Error);
        }

        return Option.Some<DeviceCodeLoginProcess, string>(new DeviceCodeLoginProcess(
            httpClient.StartPollingDeviceCodeForAuthorization(
                discovery.TokenEndpoint,
                clientId,
                deviceResponse.DeviceCode,
                deviceResponse.Interval,
                cancellationToken),
            deviceResponse.DeviceCode,
            deviceResponse.UserCode,
            new Uri(deviceResponse.VerificationUri),
            new Uri(deviceResponse.VerificationUriComplete)));
    }
    
    private static async Task<Option<TokenResponse, string>> StartPollingDeviceCodeForAuthorization(
        this HttpClient httpClient,
        string tokenEndpoint,
        string clientId,
        string deviceCode,
        int interval,
        CancellationToken cancellationToken = default)
    {
        do
        {
            var tokenResponse = await httpClient.RequestDeviceTokenAsync(new DeviceTokenRequest
            {
                Address = tokenEndpoint,
                ClientId = clientId,
                DeviceCode = deviceCode
            }, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            if (tokenResponse is { IsError: true, Error: "authorization_pending" })
            {
                await Task.Delay(Math.Clamp(interval, 1, 60) * 1000, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
            else if (tokenResponse.IsError)
            {
                return Option.None<TokenResponse, string>(tokenResponse.Error);
            }
            else
            {
                return Option.Some<TokenResponse, string>(tokenResponse);
            }
        } while (true);
    }
}