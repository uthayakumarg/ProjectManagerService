using ProjectManagerDL.EntityDataModel;
using ProjectManagerEntity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity.SqlServer;

namespace ProjectManagerDL
{
    public class TaskDataLayer : ITaskDataLayer
    {
        ProjectManagerSQLConn _db;
        public TaskDataLayer()
        {
            _db = new ProjectManagerSQLConn();
        }

        public List<ParentTaskEntity> GetParentTasks()
        {
            var tasksFromDb = (from task in _db.T_PRNT_TASK
                               select new ParentTaskEntity
                               {
                                   ParentTaskId = task.PRNT_TASK_ID,
                                   ParentTaskName = task.PRNT_TASK_NM
                               }).ToList();

            return tasksFromDb;
        }

        public void AddParentTask(ParentTaskEntity task)
        {
            var newTask = new T_PRNT_TASK();

            newTask.PRNT_TASK_NM = task.ParentTaskName;

            _db.T_PRNT_TASK.Add(newTask);
            _db.SaveChanges();
        }

        public List<TaskEntity> GetAllTasks()
        {
            var tasksFromDb = (from t in _db.T_TASK
                               join p in _db.T_PRNT_TASK on t.PRNT_TASK_ID equals p.PRNT_TASK_ID into parents
                               from parent in parents.DefaultIfEmpty()
                               join pr in _db.T_PROJ on t.PROJ_ID equals pr.PROJ_ID
                               join u in _db.T_USR on t.USR_ID equals u.EMP_ID
                               select new TaskEntity
                               {
                                   TaskId = t.TASK_ID,
                                   TaskName = t.TASK_NM,
                                   ParentId = t.PRNT_TASK_ID != null ? t.PRNT_TASK_ID.Value : 0,
                                   ParentName = parent.PRNT_TASK_NM,
                                   Priority = t.PRIORITY,
                                   StartDate = (t.STRT_DT != null ?
                                    SqlFunctions.DateName("day", t.STRT_DT) + "/" + SqlFunctions.DateName("month", t.STRT_DT) + "/" + SqlFunctions.DateName("year", t.STRT_DT) : ""),
                                   EndDate = (t.END_DT != null ?
                                    SqlFunctions.DateName("day", t.END_DT) + "/" + SqlFunctions.DateName("month", t.END_DT) + "/" + SqlFunctions.DateName("year", t.END_DT) : ""),
                                   ProjectId = t.PROJ_ID,
                                   ProjectName = pr.PROJ_NM,
                                   UserId = t.USR_ID,
                                   UserName = u.EMP_FRST_NM + " " + u.EMP_LST_NM,
                                   TaskStatus = t.STATUS
                               }).ToList();

            return tasksFromDb;
        }

        public TaskEntity GetTaskById(int taskId)
        {
            var taskFromDb = (from t in _db.T_TASK
                              join p in _db.T_PRNT_TASK on t.PRNT_TASK_ID equals p.PRNT_TASK_ID into parents
                              from parent in parents.DefaultIfEmpty()
                              join pr in _db.T_PROJ on t.PROJ_ID equals pr.PROJ_ID
                              join u in _db.T_USR on t.USR_ID equals u.EMP_ID
                              where t.TASK_ID == taskId
                              select new TaskEntity
                              {
                                  TaskId = t.TASK_ID,
                                  TaskName = t.TASK_NM,
                                  ParentId = t.PRNT_TASK_ID != null ? t.PRNT_TASK_ID.Value : 0,
                                  ParentName = parent.PRNT_TASK_NM,
                                  Priority = t.PRIORITY,
                                  StartDate = (t.STRT_DT != null ?
                                   SqlFunctions.DateName("day", t.STRT_DT) + "/" + SqlFunctions.DateName("month", t.STRT_DT) + "/" + SqlFunctions.DateName("year", t.STRT_DT) : ""),
                                  EndDate = (t.END_DT != null ?
                                   SqlFunctions.DateName("day", t.END_DT) + "/" + SqlFunctions.DateName("month", t.END_DT) + "/" + SqlFunctions.DateName("year", t.END_DT) : ""),
                                  ProjectId = t.PROJ_ID,
                                  ProjectName = pr.PROJ_NM,
                                  UserId = t.USR_ID,
                                  UserName = u.EMP_FRST_NM + " " + u.EMP_LST_NM,
                                  TaskStatus = t.STATUS
                              }).SingleOrDefault();

            return taskFromDb;
        }

        public void AddTask(TaskEntity task)
        {
            var newTask = new T_TASK();

            newTask.TASK_NM = task.TaskName;
            newTask.PRNT_TASK_ID = task.ParentId;
            newTask.PRIORITY = task.Priority;
            newTask.STRT_DT = Utility.GetFormattedDate(task.StartDate).Value;
            newTask.END_DT = Utility.GetFormattedDate(task.EndDate).Value;
            newTask.PROJ_ID = task.ProjectId;
            newTask.USR_ID = task.UserId;
            newTask.STATUS = "A"; // New tasks are Active by default

            _db.T_TASK.Add(newTask);
            _db.SaveChanges();
        }

        public void EndTask(int taskId)
        {
            var taskFromDb = (from t in _db.T_TASK
                              where t.TASK_ID == taskId
                              select t).SingleOrDefault();

            taskFromDb.STATUS = "C"; // Move to completed state

            _db.SaveChanges();
        }

        public void UpdateTask(TaskEntity task)
        {
            var taskFromDb = (from t in _db.T_TASK
                              where t.TASK_ID == task.TaskId
                              select t).SingleOrDefault();

            taskFromDb.TASK_NM = task.TaskName;
            taskFromDb.PRNT_TASK_ID = task.ParentId;
            taskFromDb.PRIORITY = task.Priority;
            taskFromDb.STRT_DT = Utility.GetFormattedDate(task.StartDate).Value;
            taskFromDb.END_DT = Utility.GetFormattedDate(task.EndDate).Value;
            taskFromDb.PROJ_ID = task.ProjectId;
            taskFromDb.USR_ID = task.UserId;

            _db.SaveChanges();
        }
    }
}
