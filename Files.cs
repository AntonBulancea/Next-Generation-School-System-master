using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Next_Generation_School_System_by_Anton {
    class Files {
        public Files(string title, string school, string Class,string date, byte[] data) {
            Title = title;
            School = school;
            Class_ = Class;
            Data = data;
            Date = date;
        }
        public string Title {
            get; private set;
        }
        public string School {
            get; private set;
        }
        public string Class_ {
            get; private set;
        }
        public string Date {
            get; private set;
        }
        public byte[] Data {
            get; private set;
        }
    }
}
