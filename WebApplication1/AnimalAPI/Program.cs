using AnimalAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initialize a list to store animals
var _animals = new List<Animal>
{
    new Animal { IdAnimal = 1, FirstName = "John", Category = "cat", Weight = 6.1, FurColour = "orange"},
    new Animal { IdAnimal = 2, FirstName = "Jane", Category = "dog", Weight = 20, FurColour = "black"},
    new Animal { IdAnimal = 3, FirstName = "Sam", Category = "bird", Weight = 0.2 , FurColour = "yellow"},
};

// Initialize a list to store animal visits
var _animalVisits = new List<AnimalVisit>();

// Endpoint to retrieve a list of animals
app.MapGet("/api/animals", () => Results.Ok(_animals))
    .WithName("GetAnimals")
    .WithOpenApi();

// Endpoint to retrieve a specific animal by id
app.MapGet("/api/animals/{id:int}", (int id) =>
{
    var animal = _animals.FirstOrDefault(s => s.IdAnimal == id);
    return animal == null ? Results.NotFound($"Animal with id {id} was not found") : Results.Ok(animal);
})
.WithName("GetAnimal")
.WithOpenApi();

// Endpoint to add an animal
app.MapPost("/api/animals", (Animal animal) =>
{
    _animals.Add(animal);
    return Results.StatusCode(StatusCodes.Status201Created);
})
.WithName("CreateAnimal")
.WithOpenApi();

// Endpoint to edit an animal
app.MapPut("/api/animals/{id:int}", (int id, Animal animal) =>
{
    var animalToEdit = _animals.FirstOrDefault(a => a.IdAnimal == id);
    if (animalToEdit == null)
    {
        return Results.NotFound($"Animal with id {id} was not found");
    }
    _animals.Remove(animalToEdit);
    _animals.Add(animal);
    return Results.NoContent();
})
.WithName("UpdateAnimal")
.WithOpenApi();

// Endpoint to delete an animal
app.MapDelete("/api/animals/{id:int}", (int id) =>
{
    var animalToDelete = _animals.FirstOrDefault(s => s.IdAnimal == id);
    if (animalToDelete == null)
    {
        return Results.NoContent();
    }
    _animals.Remove(animalToDelete);
    return Results.NoContent();
})
.WithName("DeleteAnimal")
.WithOpenApi();

// Endpoint to retrieve a list of visits associated with a given animal
app.MapGet("/api/animals/{id:int}/visits", (int id) =>
{
    var visits = _animalVisits.Where(v => v.AnimalId == id).ToList();
    return Results.Ok(visits);
})
.WithName("GetAnimalVisits")
.WithOpenApi();

// Endpoint to add a new visit for an animal
app.MapPost("/api/animals/{id:int}/visits", (int id, AnimalVisit visit) =>
{
    visit.AnimalId = id;
    _animalVisits.Add(visit);
    return Results.StatusCode(StatusCodes.Status201Created);
})
.WithName("AddAnimalVisit")
.WithOpenApi();

// Run the application
app.Run();
