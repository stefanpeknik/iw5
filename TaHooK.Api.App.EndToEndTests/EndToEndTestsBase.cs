using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaHooK.Api.BL.MapperProfiles;
using TaHooK.Api.Common.Tests;
using TaHooK.Api.Common.Tests.Factories;
using TaHooK.Api.DAL.Entities;
using TaHooK.Api.DAL.UnitOfWork;
using Xunit;

namespace TaHooK.Api.App.EndToEndTests;

public class EndToEndTestsBase : IAsyncDisposable
{
    private readonly TaHooKApiApplicationFactory application;
    protected readonly Lazy<HttpClient> client;

    public EndToEndTestsBase()
    {
        application = new TaHooKApiApplicationFactory();
        client = new Lazy<HttpClient>(application.CreateClient());
    }
    
    public async ValueTask DisposeAsync()
    {
        await application.DisposeAsync();
    }
}
