using ArcaneCollage.Entities.Interfaces;
using System;

namespace ArcaneCollage.Entities
{
    public class Time : ITime
    {
        public int year
        {
            get
            {
                return _year;
            }
            private set
            {
                _year = value;
            }
        }

        public int month
        {
            get
            {
                return _month;
            }
            private set
            {
                _month = value;
                if (_month > 12)
                {
                    year += 1;
                    _month = 1;
                }
            }
        }

        public int day
        {
            get
            {
                return _day;
            }
            private set
            {
                _day = value;
                if (_day > 30)
                {
                    month += 1;
                    _day = 1;
                }
            }
        }

        public int hour
        {
            get
            {
                return _hour;
            }
            private set
            {
                _hour = value;
                if (_hour > 23)
                {
                    day += 1;
                    _hour = 0;
                }
            }
        }

        private int _year;
        private int _month;
        private int _day;
        private int _hour;

        public void Lapse()
        {
            hour++;
        }

        public Time(int year, int month, int day, int hour)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
        }
    }
}
