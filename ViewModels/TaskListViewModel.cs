using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TaskManager.Models;
using Dapper;

namespace TaskList.ViewModels
{
    public class TaskListViewModel
    {
    public TaskListViewModel()
    {
            
        string dbConn = "data source=127.0.0.1;User id=SA;Password=Strong.Pwd-123;Initial Catalog=TaskManager";      
        using(var db = new SqlConnection(dbConn))
        {
            this.EditableItem = new TaskListItem();
            this.TaskItems = db.Query<TaskListItem>("SELECT * FROM TaskListItems ORDER BY DueDate DESC").ToList();
        }
    }

    public List<TaskListItem> TaskItems { get; set; }

    public TaskListItem EditableItem { get; set; }
    }
}