using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public interface IBook
    {
        string Title { get; set; }
        string Author { get; set; }
        int ISBN { get; set; }
        bool IsAvailable { get; set; }
    }
}
