using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://chat.openai.com", "http://localhost:5100").AllowAnyHeader().AllowAnyMethod();
    });
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HRAssistant.Plugin",
        Version = "v1",
        Description = "A plugin that provides candidate information for a company."
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.

app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
    });
});
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.yaml", "HRAssistant.Plugin v1");

});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), ".well-known")),
    RequestPath = "/.well-known"
});

/// <summary>
/// Gets the candidate information by name.
/// </summary>
app.MapGet("/personnal",  (string name) =>
{
    CandidateResponse[] testData = new CandidateResponse[]
            {
                new CandidateResponse("Karn Singh", "singh.karn@example.com", "https://www.linkedin.com/in/karnsinghprofile/"),
                new CandidateResponse("Aanya Gupta", "aanya.gupta@example.com", "https://www.linkedin.com/in/aanyagupta"),
                new CandidateResponse("Ishaan Patel", "ishaan.patel@example.com", "https://www.linkedin.com/in/ishaanpatel"),
                new CandidateResponse("Advait Desai", "advait.desai@example.com", "https://www.linkedin.com/in/advaitdesai"),
                new CandidateResponse("Neha Singh", "neha.singh@example.com", "https://www.linkedin.com/in/nehasingh"),
                new CandidateResponse("Arjun Mehta", "arjun.mehta@example.com", "https://www.linkedin.com/in/arjunmehta"),
                new CandidateResponse("Anaya Reddy", "anaya.reddy@example.com", "https://www.linkedin.com/in/anayareddy"),
                new CandidateResponse("Vivaan Joshi", "vivaan.joshi@example.com", "https://www.linkedin.com/in/vivaanjoshi"),
                new CandidateResponse("Avani Kapoor", "avani.kapoor@example.com", "https://www.linkedin.com/in/avanikapoor"),
                new CandidateResponse("Vihaan Khanna", "vihaan.khanna@example.com", "https://www.linkedin.com/in/vihaankhanna"),
                new CandidateResponse("Kavya Verma", "kavya.verma@example.com", "https://www.linkedin.com/in/kavyaverma"),
                new CandidateResponse("Dhruv Singhania", "dhruv.singhania@example.com", "https://www.linkedin.com/in/dhruvsinghania"),
                new CandidateResponse("Riya Sharma", "riya.sharma@example.com", "https://www.linkedin.com/in/riyasharma"),
                new CandidateResponse("Ishanvi Gupta", "ishanvi.gupta@example.com", "https://www.linkedin.com/in/ishanvigupta"),
                new CandidateResponse("Shaurya Kumar", "shaurya.kumar@example.com", "https://www.linkedin.com/in/shauryakumar")
            };
    CandidateResponse candidate =(CandidateResponse) testData.FirstOrDefault(c => c.name.Equals(name, StringComparison.OrdinalIgnoreCase));
    return candidate;
})
.WithName("GetPersonnalInfo")
.WithOpenApi(x =>
{
    x.Description = "Gets the candidate information by name";
    var parameter = x.Parameters[0];
    parameter.Description = "The name of the candidate.";
    return x;
});

app.Run();




internal class CandidateResponse
{
    public string name { get; set; }
    public string email { get; set; }
    public string linkedInProfile { get; set; }

    public CandidateResponse() { }
    public CandidateResponse(string name, string email, string linkedInProfile)
    {
        this.name = name;
        this.email = email;
        this.linkedInProfile = linkedInProfile;
    }
}



