using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reclone_BackEnd.Models;
using Reclone_BackEnd.Seeders;

var builder = WebApplication.CreateBuilder(args);



//Registered services

builder.Services.AddTransient<PostSeeder>();



// Add services to the container.
builder.Services.AddHttpClient("UserMicroservice", client =>
{
    client.BaseAddress = new Uri("http://localhost:43090"); // Replace with the base address of Microservice1
});

builder.Services.AddHttpClient("SearchMicroservice", client =>
{
    client.BaseAddress = new Uri(""); // Replace with the base address of Microservice2
});




//ADD DB COTEXT WITH CONNECTION STRING 
builder.Services.AddDbContext<PostDbContext>(options =>
/* options.UseSqlServer(builder.Configuration.GetSection("ConnectionString:PostDatabase").Value));*/
 options.UseSqlite("Data Source=Database/PostDb.db"));







builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





//AZURE CONTENT MODERATION

// Your Content Moderator subscription key is found in your Azure portal resource on the 'Keys' page.
var SubscriptionKey = Environment.GetEnvironmentVariable("AZURE_CONTENT_KEY");
// Base endpoint URL. Found on 'Overview' page in Azure resource. 
var Endpoint = Environment.GetEnvironmentVariable("AZURE_CONTENT_ENDPOINT");




//CLOUDINARY 

// Set your Cloudinary credentials


DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;


// Upload an image and log the response to the console

var uploadParams = new ImageUploadParams()
{
    File = new FileDescription(@"https://cloudinary-devs.github.io/cld-docs-assets/assets/images/cld-sample.jpg"),
    UseFilename = true,
    UniqueFilename = false,
    Overwrite = true
};
var uploadResult = cloudinary.Upload(uploadParams);
Console.WriteLine(uploadResult.JsonObj);



var getResourceParams = new GetResourceParams("cld-sample")
{
    QualityAnalysis = true
};
var getResourceResult = cloudinary.GetResource(getResourceParams);
var resultJson = getResourceResult.JsonObj;

// Log quality analysis score to the console
Console.WriteLine(resultJson["quality_analysis"]);



// Transform the uploaded asset and generate a URL and image tag


var myTransformation = cloudinary.Api.UrlImgUp.Transform(new Transformation()
    .Width(300).Crop("scale").Chain()
    .Effect("cartoonify"));

var myUrl = myTransformation.BuildUrl("cld-sample");
var myImageTag = myTransformation.BuildImageTag("cld-sample");

// Log the URL of the transformed asset to the console
Console.WriteLine(myUrl);

// Log the image tag for the transformed asset to the console
Console.WriteLine(myImageTag);

// SEED DATABASE WHEN APP STARTS!!
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<PostSeeder>();        
    seeder.SeedPostDB(); // Call the SeedPostDB method here
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

