using IASD.Sonoplastia.Components;
using IASD.Sonoplastia.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using MudBlazor.Services;
using QRCoder;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(System.Net.IPAddress.Parse("192.168.0.103"), 5250);
});
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();
builder.Services.AddSingleton<IInformativoTools, InformativoTools>();
builder.Services.AddSingleton<IProvaiVedeTools, ProvaiVedeTools>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/qrcode.png", async(req) =>
{
    var iserver = app.Services.GetService<IServer>();
    var addresses = iserver.Features.Get<IServerAddressesFeature>().Addresses;
    var remoteIpAddress = addresses.First();
    using QRCodeGenerator qrGenerator = new();
    using QRCodeData qrCodeData = qrGenerator.CreateQrCode(remoteIpAddress, QRCodeGenerator.ECCLevel.Q);
    using PngByteQRCode qrCode = new(qrCodeData);
    byte[] qrCodeImage = qrCode.GetGraphic(20);
    var res = req.Response;
    res.Headers.ContentType = "image/png";
    res.StatusCode = 200;
    await res.BodyWriter.WriteAsync(qrCodeImage).AsTask();
});

app.Run();
