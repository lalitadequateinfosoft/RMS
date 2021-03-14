using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagementSystem.Areas.Restaurant.Model
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int Sit { get; set; }
    }
    public class BookingData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string selecteddate { get; set; }
        public string selectedtime { get; set; }
        public string selectedslot { get; set; }
        public string Location { get; set; }
    }
    public class response
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string response_id { get; set; }
    }
}