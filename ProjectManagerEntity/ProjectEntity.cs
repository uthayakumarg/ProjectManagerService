﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagerEntity
{
    public class ProjectEntity
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TasksCount { get; set; }
        public int Completed { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Priority { get; set; }
        public int ProjectManagerId { get; set; }
        public string ProjectManagerFullName { get; set; }
    }
}
