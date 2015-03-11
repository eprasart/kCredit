using System;
using Npgsql;
using Dapper;

namespace kCredit.SM
{
    class Branch:BaseTable
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
