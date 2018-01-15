using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WorkingTask.Data;
using WorkingTask.Data.Models;

namespace WorkingTask.Services.Documents
{
    public class DocumentService : IDocumentService
    {
        private readonly TaskManageContext _taskManageContext;
        public DocumentService()
        {
            _taskManageContext = DbContextFactory.GetCurrentDbContext() as TaskManageContext;
        }
        public int AddDocument(DocumentInfo documentInfo)
        {
            var result = _taskManageContext.Add(documentInfo);
            return result.Entity.Id;
        }

        public int DelDocument(int Id)
        {
            throw new NotImplementedException();
        }

        public IList<DocumentInfo> GetDelDocumentByPage(int pageIndex, int pageSize, out int totalCount, Expression<Func<DocumentInfo, bool>> whereLambda, bool isAsc, Expression<Func<DocumentInfo, int>> orderBy)
        {
            throw new NotImplementedException();
        }

        public int UpDocument(DocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }
    }
}
