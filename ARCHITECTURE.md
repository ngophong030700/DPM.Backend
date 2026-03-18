# DPM.Backend — Claude Code Context

## Kiến trúc
- Modular Monolith, C# .NET 8
- Module hiện có: Identity (Users, Departments, Groups, Positions,...)
- Pattern chuẩn: theo Identity module

## Convention bắt buộc
- Entity kế thừa BaseEntity từ Shared
- Mỗi entity có IEntityTypeConfiguration riêng
- Repository pattern: interface ở Domain, implementation ở Infrastructure
- CQRS với MediatR: Command/Query tách biệt
- Schema database theo module: identity.*, project.*, ...
- FluentValidation cho tất cả Commands

## Ví dụ pattern chuẩn
- Xem: Modules/Identity/Domain/Users/User.cs
- Xem: Modules/Identity/Infrastructure/Persistence/Configurations/UserConfiguration.cs
- Xem: Modules/Identity/Application/Users/Commands/CreateUserCommand.cs
- Xem: Modules/Identity/Application/Users/Commands/CreateUserCommandHandler.cs
- Xem: Modules/Identity/Identity.Application/Users/Queries/GetUser/GetListUserQuery.cs
- Xem: Modules/Identity/Identity.Application/Users/Queries/GetUser/GetListUserQueryHandler.cs
- Xem: Modules/Identity/Identity.Application/Users/Mappings/UserMapping.cs
- Xem: Shared/Shared.Application/DTOs/Identities/UserDto.cs
- Xem: Modules/Identity/Identity.Application/Users/Queries/IUserQueryService.cs
- Xem: Shared/Shared.Infrastructure/QueryServices/Identities/UserQueryService.cs
- Xem: Modules/Identity/Identity.Domain/Repositories/IUserRepository.cs
- Xem: Shared/Shared.Infrastructure/Repositories/Identities/UserRepository.cs
- Xem: DPM.Backend.Host/Controllers/Identities/UserController.cs