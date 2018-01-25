using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WorkingTask.Data.BaseEnum;

namespace WorkingTask.Data.Models
{
    public class TaskGroup
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public int WorkingTaskId { get; set; }
        /// <summary>
        /// 子任务Id
        /// </summary>
        public int? WorkingTaskItemId { get; set; }
        /// <summary>
        /// 是否是开发组长
        /// </summary>
        public bool IsDevLead { get; set; }
        /// <summary>
        /// 开发人员ID
        /// </summary>
        public string DevelopId { get; set; }
        /// <summary>
        /// 人员当前状态
        /// </summary>
        public DevelopState DevelopState { get; set; }
    }
}
