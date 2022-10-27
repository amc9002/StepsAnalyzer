using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepsAnalyzer.Models
{
    public class Walker
    {
        public int Day { get; set; }
        public int Rank { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }      
        public int AmountOfSteps { get; set; }  
    }
}
