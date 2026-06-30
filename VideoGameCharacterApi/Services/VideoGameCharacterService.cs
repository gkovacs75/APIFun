using Microsoft.EntityFrameworkCore;
using VideoGameCharacterApi.Data;
using VideoGameCharacterApi.DTOs;
using VideoGameCharacterApi.Models;

namespace VideoGameCharacterApi.Services
{
    public class VideoGameCharacterService(AppDbContext context) : IVideoGameCharacterService
    {
        // An alternative way to inject the AppDbContext into the service class is to use a constructor parameter, as shown above. This allows for better testability and separation of concerns.
        // Instead of this:
        // public class VideoGameCharacterService(AppDbContext context) : IVideoGameCharacterService
        // You can use this:
        // 1. create a local var _context to hold the injected context
        // 2. Inject context into the constructor and assign it to _context

        public async Task<List<CharacterDto>> GetAllCharactersAsync() =>
            await context.Characters.Select(c => new CharacterDto { Name = c.Name, Game = c.Game, Role = c.Role }).ToListAsync();


        public async Task<CharacterDto?> GetCharacterByIdAsync(int id)
        {
            var result = await context.Characters
                .Where(c => c.Id == id)
                .Select(c => new CharacterDto
                {
                    Name = c.Name,
                    Game = c.Game,
                    Role = c.Role
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Character> AddCharacterAsync(CharacterDto characterDTO)
        {
            Character character = new Character();
            character.Name = characterDTO.Name;
            character.Game = characterDTO.Game;
            character.Role = characterDTO.Role;

            var entry = await context.Characters.AddAsync(character);
            await context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<bool> UpdateCharacterAsync(int id, CharacterDto characterDto)
        {            
            var entry = await context.Characters.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (entry != null)
            {
                entry.Name = characterDto.Name;
                entry.Game = characterDto.Game;
                entry.Role = characterDto.Role;

                int result = await context.SaveChangesAsync();

                return result > 0;
            }

            return false;
        }

        public Task<bool> DeleteCharacterAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
