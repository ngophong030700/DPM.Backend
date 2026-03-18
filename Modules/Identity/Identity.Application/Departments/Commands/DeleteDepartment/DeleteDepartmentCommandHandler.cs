using Identity.Domain.Repositories;
using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;

namespace Identity.Application.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler
        : IRequestHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IDepartmentRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public DeleteDepartmentCommandHandler(
            IDepartmentRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException("Phòng ban không tồn tại.");

            // 🔹 1. Check if has children
            if (entity.Childrens.Any(x => !x.IsDeleted))
                throw new DomainException("Không thể xóa phòng ban đang có phòng ban con.");

            // 🔹 2. Soft delete
            return await _repository.SoftDeleteAsync(request.Id, _currentUser.UserId);
        }
    }
}
