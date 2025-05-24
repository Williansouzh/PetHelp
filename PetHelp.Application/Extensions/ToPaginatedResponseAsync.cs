//using AutoMapper;
//using PetHelp.Application.Pagination;
//using System.Threading.Tasks;

//namespace PetHelp.Application.Extensions
//{
//    public static class QueryableExtensions
//    {
//        public static async Task<PaginationResponse<TDto>> ToPaginatedResponseAsync<T, TDto>(
//            this IQueryable<T> query,
//            PaginationRequest pagination,
//            IMapper mapper,
//            CancellationToken cancellationToken = default)
//        {
//            var totalCount = await query.CountAsync(cancellationToken);

//            var items = await query
//                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
//                .Take(pagination.PageSize)
//                .ToListAsync(cancellationToken);

//            var itemsDto = mapper.Map<List<TDto>>(items);

//            return new PaginationResponse<TDto>(itemsDto, totalCount, pagination.PageNumber, pagination.PageSize);
//        }
//    }
//}