using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantManagementSystem.DataModel;
using RestaurantManagementSystem.Areas.Restaurant.Model;
using RestaurantManagementSystem.Models;
using System.Globalization;
using System.Data.Entity;

namespace RestaurantManagementSystem.Areas.Restaurant.Data
{
    public class RestaurantQuery
    {
        RmsDatabaseEntities _db;
        public RestaurantQuery()
        {
            _db = new RmsDatabaseEntities();
        }

        public List<LocationEntity> checkavailability(AvailableCheck model)
        {
            List<LocationEntity> availablesit = new List<LocationEntity>();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                var date = model.StartDate.Split('/');
                var time = model.selectedtime.Split(':');
                DateTime startdate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]),Convert.ToInt32(time[0]),Convert.ToInt32(time[1]),0);
                DateTime enddate = startdate.AddMinutes(Convert.ToInt32(model.selectedslot));

                List<LocationEntity> query = new List<LocationEntity>();
                query = LocationQuery();
                if(query!=null && query.Count()>0)
                {
                    foreach(var item in query.ToList())
                    {
                        var check = _db.Bookings.Where(x => x.LocationId == item.Id && x.StartDate >= startdate && x.EndDate <= enddate).FirstOrDefault();
                        if(check==null)
                        {
                            availablesit.Add(item);
                        }
                    }
                }

            }
            catch(Exception e)
            {
                
            }
            return availablesit.OrderBy(x=>x.Id).ToList();
        }

        public List<LocationEntity> LocationQuery()
        {
            var query = (from lo in _db.Tables
                         select new LocationEntity
                         {
                             Id=lo.Id,
                             Location=lo.Location,
                             Sit=lo.Sit==null?0:lo.Sit.Value
                         }).ToList();

            return query.ToList();
        }

        public response BookTable(BookingData data)
        {
            response _response = new response();
            _response.Status = false;
            _response.Message = "Booking Failed!";

            
            _db.Database.Log = Console.Write;
            using (DbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    var date = data.selecteddate.Split('/');
                    var time = data.selectedtime.Split(':');
                    DateTime startdate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                    DateTime enddate = startdate.AddMinutes(Convert.ToInt32(data.selectedslot));

                    var _btable = data.Location.Split(',');
                    int _nooflocation = _btable.Length;
                    string _bookingref = GenerateOrderNo();
                    for (int i=0;i<_btable.Length;i++)
                    {
                        _db.Bookings.Add(new Booking {
                        BookingId= _bookingref,
                        Name=data.Name,
                        Mobile=data.Mobile,
                        Email=data.Email,
                        LocationId=Convert.ToInt32(_btable[i]),
                        DateOfBooking=DateTime.Now,
                        Slot=Convert.ToInt32(data.selectedslot),
                        CreatedDate=DateTime.Now,
                        BookingStatus=1,
                        StartDate= startdate,
                        EndDate= enddate
                        });
                        _db.SaveChanges();
                    }
                    transaction.Commit();
                    _response.Status = true;
                    _response.Message = "Booked successfully!";
                    _response.response_id = _bookingref;

                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
                return _response;
            }
        }

        public string GenerateOrderNo()
        {
            string First = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            DateTime dt = DateTime.UtcNow;
            string OrderId = "#"+First + (int)dt.DayOfWeek;
            return OrderId;
        }
    }
}