using System;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Pagination
{
    public class PagedList<T> : List<T> where T : class
    {
        public PagedList(List<T> items, int currentPage, int pageSize, int totalCount)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize); // Corrigido para usar TotalCount

            AddRange(items); // Adiciona os itens à lista
        }

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = source.Count(); // Obtém o número total de itens
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); // Paginação dos itens

            return new PagedList<T>(items, pageNumber, pageSize, totalCount); // Corrigida a ordem dos parâmetros
        }
    }
}

