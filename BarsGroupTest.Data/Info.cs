using System;

namespace BarsGroupTest.Data
{
    public class Info
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string DbName { get; set; }
        public string DbSize { get; set; }
        public DateTime? CurrentDateTime { get; set; }
    }
}
