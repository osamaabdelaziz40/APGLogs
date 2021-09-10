using System;

namespace APGFundamentals.DomainHelper.Services
{
    public static class Pager
    {
        public static bool HasMoreItems(int total, int currentPage, int pageSize) => (total - (currentPage * pageSize)) > 0;
        public static int Take(int pageSize) => pageSize;
        public static int Skip(int currentPage, int pageSize) => (currentPage - 1) * pageSize;
    }
}
