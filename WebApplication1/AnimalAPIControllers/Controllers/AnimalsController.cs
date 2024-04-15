using Microsoft.AspNetCore.Mvc;
using AnimalAPIControllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AnimalAPIControllers.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private static readonly List<Animal> _animals = new()
        {
            new Animal { IdAnimal = 1, FirstName = "John", Category = "cat", Weight = 6.1, FurColour = "orange"},
            new Animal { IdAnimal = 2, FirstName = "Jane", Category = "dog", Weight = 20, FurColour = "black"},
            new Animal { IdAnimal = 3, FirstName = "Sam", Category = "bird", Weight = 0.2 , FurColour = "yellow"},
        }; 

        private static readonly List<AnimalVisit> _animalVisits = new List<AnimalVisit>();

        [HttpGet]
        public IActionResult GetAnimals()
        {
            return Ok(_animals);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAnimal(int id)
        {
            var animal = _animals.FirstOrDefault(a => a.IdAnimal == id);
            if (animal == null)
            {
                return NotFound($"Animal with id {id} was not found");
            }
            
            return Ok(animal);
        }

        [HttpGet("{id:int}/visits")]
        public IActionResult GetAnimalVisits(int id)
        {
            var visits = _animalVisits.Where(v => v.AnimalId == id).ToList();
            return Ok(visits);
        }

        [HttpPost("{id:int}/visits")]
        public IActionResult AddAnimalVisit(int id, AnimalVisit visit)
        {
            visit.AnimalId = id;
            visit.Id = _animalVisits.Count + 1; 
            _animalVisits.Add(visit);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult CreateAnimal(Animal animal)
        {
            _animals.Add(animal);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateAnimal(int id, Animal animal)
        {
            var animalToEdit = _animals.FirstOrDefault(s => s.IdAnimal == id);

            if (animalToEdit == null)
            {
                return NotFound($"Animal with id {id} was not found");
            }
            
            _animals.Remove(animalToEdit);
            _animals.Add(animal);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAnimal(int id)
        {
            var animalToDelete = _animals.FirstOrDefault(s => s.IdAnimal == id);
            if (animalToDelete == null)
            {
                return NotFound($"Animal with id {id} was not found");
            }

            _animals.Remove(animalToDelete);
            return NoContent();
        }
    }
}
