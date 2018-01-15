using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkingTask.Data.Models
{
    public class WorkingTaskItems
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 任务项名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string ItemStyle { get; set; }
        /// <summary>
        /// 子项描述
        /// </summary>
        public string ItemRemark { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? ItemClosingDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ItemCreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public string Executor { get; set; }
        [ForeignKey("WorkingTaskId")]
        public WorkingTask WorkingTask { get; set; }
        public int WorkingTaskId { get; set; }
    }
}
