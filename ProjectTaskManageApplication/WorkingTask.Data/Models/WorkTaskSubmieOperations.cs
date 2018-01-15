using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkingTask.Data.Models
{
    public class WorkTaskSubmieOperations
    {
        [Key]
        public int Id { get; set; }
        public int WorkTaskId { get; set; }
        public int WorkingTaskItemId { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskState { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string ItemStyle { get; set; }
        /// <summary>
        /// 子项描述
        /// </summary>
        public string ItemRemark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ItemCreateTime { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string Executor { get; set; }

    }
}
