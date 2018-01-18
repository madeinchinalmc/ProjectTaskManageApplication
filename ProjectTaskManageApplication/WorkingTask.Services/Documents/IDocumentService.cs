using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.Documents
{
    public interface IDocumentService
    {
        int AddDocument(DocumentInfo documentInfo);
        Task UpDocument(DocumentInfo documentInfo);
        Task DelDocument(int Id);
        IList<DocumentInfo> GetDocumentByPage(int pageIndex, int pageSize, out int totalCount,
            Expression<Func<DocumentInfo, bool>> whereLambda, bool isAsc, Expression<Func<DocumentInfo, int>> orderBy);
    }
}
