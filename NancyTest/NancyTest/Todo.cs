using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoNancy
{
    public class Todo
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public bool Completed { get; set; }
    }
}