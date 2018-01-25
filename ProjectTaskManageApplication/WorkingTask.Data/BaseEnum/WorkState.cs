using System;
using System.Collections.Generic;
using System.Text;

namespace WorkingTask.Data.BaseEnum
{
    /// <summary>
    /// 任务状态 
    /// </summary>
    public enum WorkTaskState
    {
        Establish,//创建
        Start,//开始
        Distribution,//分配
        Working,//进行中
        Finish //完成
    }
}
