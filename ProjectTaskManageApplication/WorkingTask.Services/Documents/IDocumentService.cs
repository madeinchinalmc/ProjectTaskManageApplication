using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.Documents
{
    public interface IDocumentService
    {
        int AddDocument(DocumentInfo documentInfo);

        int UpDocument(DocumentInfo documentInfo);
        int DelDocument(int Id);
        IList<DocumentInfo> GetDelDocumentByPage(int pageIndex, int pageSize, out int totalCount,
            Expression<Func<DocumentInfo, bool>> whereLambda, bool isAsc, Expression<Func<DocumentInfo, int>> orderBy);
    }
}
