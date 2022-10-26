using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_project.Dtos.Character;
using AutoMapper;
using dotnet_project.Data;
using Microsoft.EntityFrameworkCore;


namespace dotnet_project.Services.CharacterService
{

    public class CharacterService: ICharacterService
    {

         private static List<Character> characters = new List<Character> {
            new Character(),
            new Character {Id = 1, Name = "Sam"},
            new Character {Id = 2, Name = "Peter"}
        };

        private readonly IMapper _mapper;

        private readonly DataContext _context;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter) 
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = (_mapper.Map<Character>(newCharacter));
            characters.Add(_mapper.Map<Character>(newCharacter));
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

             return response;
        }
    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

        try{
        Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

        _mapper.Map(updatedCharacter, character);

        // character.Name = updatedCharacter.Name;
        // character.HitPoints = updatedCharacter.HitPoints;
        // character.Strength = updatedCharacter.Strength;
        // character.Intelligence = updatedCharacter.Intelligence;
        // character.Class = updatedCharacter.Class;

        response.Data = _mapper.Map<GetCharacterDto>(character);
        } catch(Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
        

        try
        {
            Character character = characters.First(c => c.Id == id);
            characters.Remove(character);
            response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    }
}