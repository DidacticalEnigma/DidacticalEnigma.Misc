using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Optional;

namespace DidacticalEnigma.Mem.Client;

public class DeviceCodeLoginProcess
{
    public Task<Option<TokenResponse, string>> ProcessResultTask { get; }
    
    public string DeviceCode { get; }
    
    public string UserCode { get; }
    
    public Uri VerificationUri { get; }
    
    public Uri VerificationUriComplete { get; }

    public DeviceCodeLoginProcess(
        Task<Option<TokenResponse, string>> processResultTask,
        string deviceCode,
        string userCode,
        Uri verificationUri,
        Uri verificationUriComplete)
    {
        ProcessResultTask = processResultTask ?? throw new ArgumentNullException(nameof(processResultTask));
        DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
        UserCode = userCode ?? throw new ArgumentNullException(nameof(userCode));
        VerificationUri = verificationUri ?? throw new ArgumentNullException(nameof(verificationUri));
        VerificationUriComplete =
            verificationUriComplete ?? throw new ArgumentNullException(nameof(verificationUriComplete));
    }
}