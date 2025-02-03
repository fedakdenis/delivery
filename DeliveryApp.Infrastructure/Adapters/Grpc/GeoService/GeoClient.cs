using DeliveryApp.Core.Ports;
using GeoApp.Api;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Options;
using Location = DeliveryApp.Core.Domain.SharedKernel.Location;


namespace DeliveryApp.Infrastructure.Adapters.Grpc.GeoService;

public class GeoClient : IGeoClient
{
    private readonly string _url;
    private readonly SocketsHttpHandler _socketsHttpHandler;
    private readonly MethodConfig _methodConfig;

    public GeoClient(IOptions<Settings> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.GeoServiceGrpcHost))
        { 
            throw new ArgumentException(nameof(options.Value.GeoServiceGrpcHost));
        }
        
        _url = options.Value.GeoServiceGrpcHost;

        _socketsHttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };

        _methodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };
    }

    public async Task<Location> GetGeolocationAsync(string street, CancellationToken cancellationToken)
    {
        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = _socketsHttpHandler,
            ServiceConfig = new ServiceConfig { MethodConfigs = { _methodConfig } }
        };
        using (var channel = GrpcChannel.ForAddress(_url, channelOptions))
        {
            var client = new Geo.GeoClient(channel);
            var request = new GetGeolocationRequest
            {
                Street = street
            };
            var deadline = DateTime.UtcNow.AddSeconds(2);
            var reply = await client.GetGeolocationAsync(request, null, deadline, cancellationToken);

            var location = new Location(reply.Location.X, reply.Location.Y);
            return location;
        }
    }
}
