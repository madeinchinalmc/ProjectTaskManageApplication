using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkingTask.Data.Models
{
    public class WorkingTask
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string WorkingTaskName { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskState { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskRemark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<WorkingTaskItems> WorkingTaskItems { get; set; }
    }
}
