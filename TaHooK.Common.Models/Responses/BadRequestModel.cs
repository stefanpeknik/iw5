namespace TaHooK.Common.Models.Responses;

public record BadRequestModel
{
    public required List<string> Errors { get; set; }
}