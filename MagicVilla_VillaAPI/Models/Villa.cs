using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace MagicVilla_VilliAPI.Models
{
	public class Villa 
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
