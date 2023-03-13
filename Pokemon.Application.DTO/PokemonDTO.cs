﻿namespace Pokemon.Application.DTO
{
    public class PokemonDTO
    {
        public PokemonDTO()
        {

        }
        public PokemonDTO(int id, string name, int captureRate)
        {
            this.Id = id;
            Name = name;
            this.Capture_Rate = captureRate;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<PokemonTypeDTO> Types { get; set; }
        public SpeciesDTO Species { get; set; }
        public PokemonEvolutionChainLinkResponseDTO Evolution_Chain { get; set; }
        public string SpriteUrl => $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png";
        public float Capture_Rate { get; set; }
        public SpritesDTO Sprites { get; set; }
        public List<EvolutionDTO> Evolutions { get; set; }
    }

    public class SpritesDTO
    {
        public string Front_Default { get; set; }
    }

    public class SpeciesDTO
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class PokemonTypeDTO
    {
        public int Slot { get; set; }
        public PokemonTypeDetailDTO Type { get; set; }
    }

    public class PokemonTypeDetailDTO
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

}