using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        public async Task DelDocument(int Id)
        {
            var entityDocument = await _taskManageContext.First<DocumentInfo>(t => t.Id == Id);
            await _taskManageContext.Del<DocumentInfo>(entityDocument);
        }

        public  IList<DocumentInfo> GetDocumentByPage(int pageIndex, int pageSize, out int totalCount, Expression<Func<DocumentInfo, bool>> whereLambda, bool isAsc, Expression<Func<DocumentInfo, int>> orderBy)
        {
            var entitys = _taskManageContext.PageAsQuery<DocumentInfo, int>(pageIndex, pageSize,out totalCount, whereLambda, isAsc, orderBy);
            return entitys.ToList();
        }

        public async Task UpDocument(DocumentInfo documentInfo)
        {
            await _taskManageContext.Up<DocumentInfo>(documentInfo);
        }
    }
}
