using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public interface IReader
    {
        string Name { get; set; }
        List<Book> BorrowedBooks { get; set; }
    }
}
