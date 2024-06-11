using DotNetLibrary.Models.Entities;

namespace DotNetLibrary.Application.Exceptions;

public class ForbiddenException(UserRole requesterRole, string forbiddenAction)
    : Exception($"Role {requesterRole} cannot {forbiddenAction}");