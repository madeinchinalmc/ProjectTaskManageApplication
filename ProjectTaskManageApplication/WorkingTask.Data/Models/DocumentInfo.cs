using System;
using System.Collections.Generic;
using System.Text;

namespace WorkingTask.Data.Models
{
    /// <summary>
    /// 文档
    /// </summary>
    public class DocumentInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string FilePath { get; set; }
    }
}
