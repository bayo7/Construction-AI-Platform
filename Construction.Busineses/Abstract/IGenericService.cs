using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.Abstract
{
    public interface IGenericService<T>
    {
        Task TInsertAsync(T entity);
        Task TDeleteAsync(T entity);
        Task TUpdateAsync(T entity);
        Task<T> TGetByIdAsync(int id);
        Task<List<T>> TGetListAsync();
    }
}
