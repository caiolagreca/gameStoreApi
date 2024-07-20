using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGenreDto(
    [Required] string Name
 );
