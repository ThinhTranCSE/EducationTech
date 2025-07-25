﻿namespace EducationTech.Business.Abstract
{
    public interface IPagination<TRequestDto, TResponseDto>
        where TRequestDto : class
        where TResponseDto : class
    {
        Task<TResponseDto> GetPaginatedData(TRequestDto requestDto, int? offset, int? limit, string? cursor);
        Task<int> GetTotalCount();
    }
}
