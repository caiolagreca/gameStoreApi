using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateGenreDto(
    [Required] string Name
);
