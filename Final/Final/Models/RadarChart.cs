using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public class RadarChart
    {
                public string label{get;set;}
				public string fillColor {get;set;}				
                public string strokeColor{get;set;}
				public string pointColor{get;set;}
				public string  pointStrokeColor {get;set;}
				public string pointHighlightFill{get;set;}
                public string pointHighlightStroke { get; set; }
				public List<int> data{get;set;}
    }
}